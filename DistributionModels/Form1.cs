using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;
using System.Threading;

namespace DistributionModels
{
    public partial class Form1 : Form
    {

        bool startModelingFlag = true;
        private bool hasDeviate;
        private double storedSigma;
        Random myRandom = new Random();
        double muRecovery = 0;
        double lambdaRecovery = 0;
        double sigmaRecovery = 0;
        double muFailure = 0;
        double lambdaFailure = 0;
        double sigmaFailure = 0;
        int countIteration = 0;
        /*Время моделирования*/
        double currentTimeSimulation = 0;
        double timeSimulation = 0;
        /*Количество состояний*/
        int countOfStates = 0;
        double countInfelicity = 0.01;
        /*Сумма значений нерабочих состояний*/
        double valueOfNonWorkingStates = 0;
        /*Сумма значения рабочих состояний*/
        double valueOfWorkingStates = 0;
        /*Количество коэффциентов готоаности*/
        int countOfAvailabilityFactor = 0;
        /*Массив значений коэффициентов готовности*/
        List<double> availabilityFactor = new List<double>(1000);
        List<double> resultsRecovery = new List<double>(1000);
        List<double> resultsFailure = new List<double>(1000);
        /*Сумма состояний*/
        double sumOfStates = 0;
        /*Состояния*/
        List<List<double>> states = new List<List<double>>(10000);
        delegate void Modeling(List<List<double>> states, ref int countOfStates, List<double> resultsNormalDistrb,
            List<double> resultsExponentialDistrb, double muRecovery, double sigmaRecovery, double lambdaFailure);
        delegate void Modeling2(List<List<double>> states, ref int countOfStates, List<double> resultsNormalDistrb,
            List<double> resultsExponentialDistrb, double lambdaFailure, double lambdaRecovery);
        delegate void Modeling3(List<List<double>> states, ref int countOfStates, List<double> resultsNormalDistrb,
            List<double> resultsExponentialDistrb, double muFailure, double sigmaFailure, double muRecovery, double sigmaRecovery);
        delegate void Modeling4(List<List<double>> states, ref int countOfStates, List<double> resultsNormalDistrb,
            List<double> resultsExponentialDistrb, double muFailure, double sigmaFailure, double lambdaRecovery);
        public Form1()
        {
            InitializeComponent();
        }
        /*Нормальное распределение*/
        private double NormalDistribution(double mu = 0, double sigma = 1)
        {
            if (hasDeviate)
            {
                /*Обработка исключения при отрицательном среднекваратичном отклонении*/
                if (sigma <= 0)
                {
                    throw new ArgumentOutOfRangeException("Среднекваратичное отклонение(sigma) должно быть положительным");

                }
                hasDeviate = false;
                //Возвращаем первое отклонение
                return storedSigma * sigma + mu;
            }
            double v1, v2, rSquared;
            do
            {
                //Рассчитаем два рандомных числа в диапазоне [-1;1]
                v1 = 2 * myRandom.NextDouble() - 1;
                v2 = 2 * myRandom.NextDouble() - 1;
                rSquared = v1 * v1 + v2 * v2;
                //Внутри круга
            } while (rSquared >= 1 || rSquared == 0);

            //Рассчитаем полярное преобразование для каждого отклонения
            var polar = Math.Sqrt(-2 * Math.Log(rSquared) / rSquared);
            storedSigma = v2 * polar;
            hasDeviate = true;
            //Возвращаем второе отклонение
            return v1 * polar * sigma + mu;
        }
        /*Экспоненциальное распределение*/
        private double ExponentialDistribution(Random r, double l)
        {
            // return (Math.Log(1-r.NextDouble())/(-l));
            //return -1 / (l * Math.Log(r.NextDouble()));
            return (1 / (Math.Log(1 - r.NextDouble()) / (-l)));
        }
        private void DrawGraph(uint[] resultSimulation, int countOfChanges)
        {
            GraphPane graphPane = zedGraphControl1.GraphPane;
            graphPane.Title.Text = "Моделирование одного элемента";
            PointPairList list1 = new PointPairList();
            list1.Add(0, resultSimulation[0]);
            for (int i = 1; i < countOfChanges - 1; i++)
            {
                if (resultSimulation[i] == resultSimulation[i - 1])
                {
                    list1.Add(i, resultSimulation[i]);
                }

                else if (resultSimulation[i] == 0 || resultSimulation[i] == 1)
                {
                    list1.Add(i - 1, resultSimulation[i]);
                }
                if (resultSimulation[i - 1] == 0 || resultSimulation[i - 1] == 1)
                {
                    list1.Add(i, resultSimulation[i]);
                }
            }
            string label = "Состояния элемента \nВерхний горизон - интервалы восстановления\nНижний горизонт - интервалы работоспособности";
            LineItem myCurve1 = graphPane.AddCurve(label, list1, Color.Red, SymbolType.None);

            /* По оси Y установим автоматический подбор масштаба*/
            graphPane.YAxis.Scale.MinAuto = true;
            graphPane.YAxis.Scale.MaxAuto = true;
            graphPane.YAxis.MajorGrid.IsZeroLine = false;
            graphPane.YAxis.Title.Text = "Рабочее либо нерабочее состояние";
            graphPane.XAxis.Title.Text = "Количество состояний за момент времени t";
            /*Установим значение параметра IsBoundedRanges как true.
              Это означает, что при автоматическом подборе масштаба 
              нужно учитывать только видимый интервал графика*/
            graphPane.IsBoundedRanges = true;
            /*Обновляем данные об осях*/
            graphPane.AxisChange();
            /*Обновляем график*/
            zedGraphControl1.Invalidate();
        }
        private void DrawHistogram(List<double> results)
        {
            int countRange = 10;
            double min = -1000;
            double max = 500;
            double rangeSize = (max - min) / countRange;
            results.Sort();
            results.GroupBy(x => (int)((x - min) / rangeSize)).Select(x => x.Count()).ToList();
            // Получим панель для рисования
            GraphPane pane = zedGraphControl1.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            double[] values = new double[results.Count];
            results.CopyTo(values);

            // Создадим кривую-гистограмму
            // Первый параметр - название кривой для легенды
            // Второй параметр - значения для оси X, т.к. у нас по этой оси будет идти текст, а функция ожидает тип параметра double[], то пока передаем null
            // Третий параметр - значения для оси Y
            // Четвертый параметр - цвет
            BarItem curve = pane.AddBar("Гистограмма функции распределния нормального распределения", values, null, Color.Blue);
            // Настроим ось X так, чтобы она отображала текстовые данные
            pane.XAxis.Type = ZedGraph.AxisType.Text;
            // Уставим для оси наши подписи
            //pane.XAxis.Scale.TextLabels = names;
            pane.BarSettings.MinClusterGap = 0.0f;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraphControl1.AxisChange();
            // Обновляем график
            zedGraphControl1.Invalidate();
        }
        private void SaveToFileFirst(string path, List<double> results)
        {
            FileStream aFile = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile, Encoding.Default);
            foreach (double result in results)
            {
                sw.WriteLine(result);
            }
            sw.Close();
        }

        private void SaveToFileSecond(string path, List<List<double>> results)
        {
            FileStream aFile = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile, Encoding.Default);
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i][1] != 0)
                {
                    sw.WriteLine(results[i][0] + " " + results[i][1]);
                }
                else
                {
                    break;
                }

            }
            sw.Close();
        }
        /*Метод для отрисовки графика коэффициента готовности*/
        private void DrawAvailabilityFactor(List<double> availabilityFactor, double timeSimulation)
        {
            GraphPane graphPane = zedGraphControl1.GraphPane;
            graphPane.Title.Text = "Моделирование элемента";
            PointPairList list1 = new PointPairList();
            // list1.Add(0, availabilityFactor[0]);
            double g = timeSimulation / (double)availabilityFactor.Count;
            double j = g;
            int p = 0;
            double b = 0;
            double a = 0;
            //list1.Add(0, availabilityFactor[0]);
            //j = j + g;
            //list1.Add(j, availabilityFactor[1]);
            //double min = availabilityFactor[1];
            for (int i = 0; i < availabilityFactor.Count; i++)
            {
                b += availabilityFactor[i];

            }
            b = b / (availabilityFactor.Count);
            b -= 0.0001;
            for (int i = availabilityFactor.Count - 2; i >= 0; i--)
            {
                if (Math.Abs(b - availabilityFactor[i]) >= 0.04 || timeSimulation<=5000)
                { 
                    goto Leave;
                }
                else
                {
                    if (Math.Abs(b - availabilityFactor[i]) >= 0.003)
                    {
                        break;
                    }
                    else
                    {
                        goto Leave;
                    }

                }
            }
            for (int i = availabilityFactor.Count - 1; i > 0; i--)
            {
                if (availabilityFactor[i] <= b)
                {
                    availabilityFactor.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

        // Math.Truncate(b * 1000);
        // b=Math.Round(b, 3)-0.002;
        Leave: if (timeSimulation >= 10000 || radioButtonInfelicity.Checked==true)
            {
                for (int i = 0; i < availabilityFactor.Count; i++)///i < sumOfStates;i+=360) //&& availabilityFactor[i] - availabilityFactor[i - 1] <= 0.01; i++)
                {
                    if (i != availabilityFactor.Count - 5)
                    {
                        list1.Add(j, availabilityFactor[i]);
                        j += g;
                    }
                    else
                    {
                        while (p != 5)
                        {
                            b = Math.Truncate(availabilityFactor[i] * 100000);
                            b = b / 100000;
                            list1.Add(j, b);
                            j = j + g;
                            p++;
                        }
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < availabilityFactor.Count; i++)///i < sumOfStates;i+=360) //&& availabilityFactor[i] - availabilityFactor[i - 1] <= 0.01; i++)
                {

                    list1.Add(j, availabilityFactor[i]);
                    j += g;

                }


                //if (Math.Abs(availabilityFactor[i]-availabilityFactor[i+1]) >=0.01)
                //{

                //}
                //else
                //{
                //    break;
                //}
                //else
                //{
                //    if (Math.Abs(availabilityFactor[i] - availabilityFactor[i + 1])<= 0.01)
                //    {
                //       

                //    }

            }



            //while(Math.Abs(min - availabilityFactor[i]) <= countInfelicity && i<availabilityFactor.Count-8)
            //{
            //    i++;
            //}
            //// if (Math.Abs(availabilityFactor[i] - availabilityFactor[i-1]) >= 0.000001)

            //// {
            //min = availabilityFactor[i];
            //    j = j + g;
            //    list1.Add(j, availabilityFactor[i]);
            //if (i == availabilityFactor.Count - 8)
            //{
            //    while (p != 5)
            //    {
            //        j = j + g;
            //        i++;
            //        b = availabilityFactor[i];
            //        // b = b / countInfelicity;
            //        list1.Add(j, b);
            //        p++;
            //    }
            //    break;
            //}
            //}
            //else
            //{
            //    while (p != 5)
            //    {
            //        j = j + g;
            //        i++;
            //        b = Math.Truncate(availabilityFactor[i] * 1000);
            //        b = b / 1000;
            //        list1.Add(j, b);                       
            //        p++;
            //    }
            //    break;
            //}



            LineItem myCurve1 = graphPane.AddCurve("Коэффициенты готовности", list1, Color.Red, SymbolType.None);
            myCurve1.Line.IsSmooth = true;
            myCurve1.Line.SmoothTension = 0.5f;

            /* По оси Y установим автоматический подбор масштаба*/
            graphPane.YAxis.Scale.MinAuto = true;
            graphPane.YAxis.Scale.MaxAuto = true;
            graphPane.YAxis.MajorGrid.IsZeroLine = false;
            graphPane.YAxis.Title.Text = "Коэффициент готовности";
            graphPane.XAxis.Title.Text = "Время (в часах)";
            /*Установим значение параметра IsBoundedRanges как true.
              Это означает, что при автоматическом подборе масштаба 
              нужно учитывать только видимый интервал графика*/
            graphPane.IsBoundedRanges = true;
            /*Обновляем данные об осях*/
            graphPane.AxisChange();
            /*Обновляем график*/
            zedGraphControl1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timeSumulationUserBox.Text = "1000";
            muRecoveryUserBox.Text = "5";
            sigmaRecoveryUserBox.Text = "1";
            muFailureUserBox.Text = "100";
            sigmaFailureUserBox.Text = "5";
            CountIterationBox.Text = "100000";
            InfelicityUserBox.Text = "0,001";
            CountIterationBox.MaxLength = 7;
            menuStrip1.Enabled = false;
            workLabel.Visible = false;
            toolStripProgressBar1.Visible = false;
            comboBox1.Items.Add("Экспоненциальное");
            comboBox1.Items.Add("Нормальное");
            comboBox2.Items.Add("Экспоненциальное");
            comboBox2.Items.Add("Нормальное");
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
            StartModelingBtn.Enabled = true;
            radioButtonTime.Checked = true;
        }
        /*Обработчмк нажатмя на кнопку "Начать моделирование"*/
        private async void StartModelingBtn_Click(object sender, EventArgs e)
        {

            Modeling modeling = null;
            Modeling2 modeling2 = null;
            Modeling3 modeling3 = null;
            Modeling4 modeling4 = null;
            if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 1)
            {
                modeling = ModelingExponentialNormal;
            }
            else if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 0)
            {
                modeling2 = ModelingExponentialExponential;
                //panel3.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 1)
            {
                modeling3 = ModelingNormalNormal;
                //panel4.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 0)
            {
                modeling4 = ModelingNormalExponential;
            }
            if (startModelingFlag == false)
            {
                clearGraph();
            }
            if (timeSumulationUserBox.Enabled == true)
            {

                timeSimulation = Convert.ToDouble(timeSumulationUserBox.Text);

            }
            else
            {
                countInfelicity = Convert.ToDouble(InfelicityUserBox.Text);
            }

            if (muRecoveryUserBox.Enabled == true)
            {
                muRecovery = Convert.ToDouble(muRecoveryUserBox.Text);
            }
            if (sigmaRecoveryUserBox.Enabled == true)
            {
                sigmaRecovery = Convert.ToDouble(sigmaRecoveryUserBox.Text);
            }
            if (lambdaRecoveryUserBox.Enabled == true)
            {
                lambdaRecovery = Convert.ToDouble(lambdaRecoveryUserBox.Text);
            }
            if (muFailureUserBox.Enabled == true)
            {
                muFailure = Convert.ToDouble(muFailureUserBox.Text);
            }
            if (sigmaFailureUserBox.Enabled == true)
            {
                sigmaFailure = Convert.ToDouble(sigmaFailureUserBox.Text);
            }
            if (lambdaFailureUserBox.Enabled == true)
            {
                lambdaFailure = Convert.ToDouble(lambdaFailureUserBox.Text);
            }

            countIteration = Convert.ToInt32(CountIterationBox.Text);
            if (CheckLogic() == false)
            {
                return;
            }
            
            StartModelingBtn.Enabled = false;
            panel1.Enabled = false;
            panel2.Enabled = false;
            menuStrip1.Enabled = false;
            workLabel.Text = "Идет расчет...";
            workLabel.Visible = true;
            toolStripProgressBar1.Maximum = countIteration + 10;
            toolStripProgressBar1.Visible = true;

            for (int k = 0; k < 1000000; k++)
            {
                availabilityFactor.Add(0);
            }
            for (int k = 0; k < 1500000; k++)
            {
                states.Add(new List<double> { 0, 0 });
            }
            int b = 0;
            if (radioButtonTime.Checked)
            {
                /*Собираем статистику*/
                for (int i = 0; i < countIteration; i++)
                {
                    await Task.Delay(10);
                    toolStripProgressBar1.Value++;
                    b += countOfAvailabilityFactor;
                    countOfStates = 0;
                    resultsRecovery.Clear();
                    resultsFailure.Clear();                  
                    /*Обнуляем время моделирования*/
                    currentTimeSimulation = 0;
                    sumOfStates = 0;
                    countOfAvailabilityFactor = 0;
                    valueOfNonWorkingStates = 0;
                    valueOfWorkingStates = 0;
                    /*Пока время моделирования меньше заданного числа - моделируем*/
                    // while (currentTimeSimulation < timeSimulation)
                    //{
                    if (modeling == null)
                    {
                        if (modeling2 == null)
                        {
                            if (modeling3 == null)
                            {
                                if (modeling4 == null)
                                {

                                }
                                else
                                {
                                    modeling4(states, ref countOfStates, resultsRecovery, resultsFailure,
                                        muFailure, sigmaFailure, lambdaRecovery);
                                }
                            }
                            else
                            {
                                modeling3(states, ref countOfStates, resultsRecovery, resultsFailure, muFailure,
                                    sigmaFailure, muRecovery, sigmaRecovery);
                            }
                        }
                        else
                        {
                            modeling2(states, ref countOfStates, resultsRecovery, resultsFailure, lambdaFailure, lambdaRecovery);
                        }
                    }
                    else
                    {
                        modeling(states, ref countOfStates, resultsRecovery, resultsFailure, muRecovery, sigmaRecovery, lambdaFailure);
                    }
                    // modeling(states, ref countOfStates, resultsNormalDistrb, resultsExponentialDistrb, muRecovery, sigmaRecovery, lambdaRecovery);
                    // Modeling(states, ref countOfStates, resultsNormalDistrb, resultsExponentialDistrb,mu,sigma,lambda);
                    //for (int p = 0; p <= countOfStates; p++)
                    //{
                    //    if (currentTimeSimulation < timeSimulation)
                    //    {
                    //        currentTimeSimulation += states[p][1];
                    //    }

                    //}
                    // }

                    /*Считаем коэффициент готовности*/
                    for (int j = 0; j < countOfStates;)
                    {

                        /*Если текущее состояние = 0, то сумируем время отказа*/
                        if (states[j][0] != 0 && j < countOfStates)
                        {
                            valueOfWorkingStates += states[j][1];
                            j++;
                        }
                        /*Иначе если текущее состояние = 1, то суммируем время восстановиления*/
                        else if (states[j][0] != 1 && j < countOfStates)
                        {
                            valueOfNonWorkingStates += states[j][1];
                            j++;

                        }
                        /*Если первый раз считаем коэффициент готовности*/
                        if (sumOfStates == 0)
                        {
                            /*Расчет коэффициента готовности*/
                            sumOfStates = valueOfNonWorkingStates + valueOfWorkingStates;
                            availabilityFactor[countOfAvailabilityFactor] += valueOfWorkingStates / sumOfStates;
                            countOfAvailabilityFactor++;
                        }

                        else if (sumOfStates > 0)//&& sumOfStates < currentTimeSimulation) //&& sumOfStates<3600000)// && sumOfStates<timeSimulation)
                        {
                            /*Расчет коэффициента готовности*/
                            sumOfStates = valueOfNonWorkingStates + valueOfWorkingStates;
                            availabilityFactor[countOfAvailabilityFactor] += valueOfWorkingStates / sumOfStates;
                            countOfAvailabilityFactor++;

                        }

                    }


                }
                availabilityFactor.RemoveAll(item => item == 0);
                b /= countIteration;
                countOfAvailabilityFactor = b;
                for (int i = 0; i < availabilityFactor.Count; i++)
                {
                    availabilityFactor[i] /= countIteration;

                }
                availabilityFactor.RemoveRange(countOfAvailabilityFactor, availabilityFactor.Count - countOfAvailabilityFactor);
                double result = 0;
                for (int i = 0; i < countOfStates; i += 2)
                {

                    result = result + states[i][1];
                }
               result /= countOfStates;
            }
            else
            {
                int g = 0;
                timeSimulation = 1000;

            Start:   /*Собираем статистику*/
                for (int i = 0; i < countIteration; i++)
                {
                    await Task.Delay(10);
                    toolStripProgressBar1.Value++;
                    b += countOfAvailabilityFactor;
                    countOfStates = 0;
                    resultsRecovery.Clear();
                    resultsFailure.Clear();
                    /*Обнуляем время моделирования*/
                    sumOfStates = 0;
                    countOfAvailabilityFactor = 0;
                    valueOfNonWorkingStates = 0;
                    valueOfWorkingStates = 0;
                    /*Пока время моделирования меньше заданного числа - моделируем*/
                    // while (currentTimeSimulation < timeSimulation)
                    //{
                    if (modeling == null)
                    {
                        if (modeling2 == null)
                        {
                            if (modeling3 == null)
                            {
                                if (modeling4 == null)
                                {

                                }
                                else
                                {
                                    modeling4(states, ref countOfStates, resultsRecovery, resultsFailure,
                                        muFailure, sigmaFailure, lambdaRecovery);
                                }
                            }
                            else
                            {
                                modeling3(states, ref countOfStates, resultsRecovery, resultsFailure, muFailure,
                                    sigmaFailure, muRecovery, sigmaRecovery);
                            }
                        }
                        else
                        {
                            modeling2(states, ref countOfStates, resultsRecovery, resultsFailure, lambdaFailure, lambdaRecovery);
                        }
                    }
                    else
                    {
                        modeling(states, ref countOfStates, resultsRecovery, resultsFailure, muRecovery, sigmaRecovery, lambdaFailure);
                    }

                    /*Считаем коэффициент готовности*/
                    for (int j = 0; j < countOfStates;)
                    {

                        /*Если текущее состояние = 0, то сумируем время отказа*/
                        if (states[j][0] != 0 && j < countOfStates)
                        {
                            valueOfWorkingStates += states[j][1];
                            j++;
                        }
                        /*Иначе если текущее состояние = 1, то суммируем время восстановиления*/
                        else if (states[j][0] != 1 && j < countOfStates)
                        {
                            valueOfNonWorkingStates += states[j][1];
                            j++;

                        }
                        /*Если первый раз считаем коэффициент готовности*/
                        if (sumOfStates == 0)
                        {
                            /*Расчет коэффициента готовности*/
                            sumOfStates = valueOfNonWorkingStates + valueOfWorkingStates;
                            availabilityFactor[countOfAvailabilityFactor] += valueOfWorkingStates / sumOfStates;
                            countOfAvailabilityFactor++;
                        }

                        else if (sumOfStates > 0)//&& sumOfStates < currentTimeSimulation) //&& sumOfStates<3600000)// && sumOfStates<timeSimulation)
                        {
                            /*Расчет коэффициента готовности*/
                            sumOfStates = valueOfNonWorkingStates + valueOfWorkingStates;
                            availabilityFactor[countOfAvailabilityFactor] += valueOfWorkingStates / sumOfStates;
                            countOfAvailabilityFactor++;

                        }
                        g = j;
                    }
                }
                for (int i = 0; i < availabilityFactor.Count; i++)
                {
                    if (availabilityFactor[i] != 0)
                    {
                        availabilityFactor[i] /= countIteration;
                    }
                    else
                    {
                        break;
                    }
                    

                }
                int temp = 0;
                b /= countIteration;
                temp = Math.Abs(countOfAvailabilityFactor - b);
                if (g >= countOfStates)
                {
                    for (int t = countOfAvailabilityFactor-temp-2; t > 0; t--)///i < sumOfStates;i+=360) //&& availabilityFactor[i] - availabilityFactor[i - 1] <= 0.01; i++)
                    {
                        if (Math.Abs(availabilityFactor[t] - availabilityFactor[t - 1]) >= countInfelicity)
                        {
                            timeSimulation += 1000;
                            toolStripProgressBar1.Maximum += countIteration;
                            goto Start;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                availabilityFactor.RemoveAll(item => item == 0);
                countOfAvailabilityFactor = b;
                availabilityFactor.RemoveRange(countOfAvailabilityFactor, availabilityFactor.Count - countOfAvailabilityFactor);
            }

            startModelingFlag = false;
            
            workLabel.Text = "Сохранение в файлы...";
            await Task.Delay(2);            
            SaveToFileFirst("Коэффициенты готовности.txt", availabilityFactor);
            SaveToFileSecond("Результаты моделирования(время и состояния).txt", states);
            toolStripProgressBar1.Value += 5;
            SaveToFileFirst("Результаты восстановления.txt", resultsRecovery);
            SaveToFileFirst("Результаты отказа.txt", resultsFailure);
            DrawAvailabilityFactor(availabilityFactor, timeSimulation);
            availabilityFactor.Clear();
            states.Clear();
            await Task.Delay(2);
            toolStripProgressBar1.Value += 5;
            workLabel.Text = "Расчет окончен, результаты сохранены в файлы";
            StartModelingBtn.Enabled = true;
            menuStrip1.Enabled = true;
            panel1.Enabled = true;
            panel2.Enabled = true;
        }
        private void ModelingExponentialNormal(List<List<double>> states, ref int countOfStates, List<double> resultsRecovery,
            List<double> resultsFailure, double muRecovery, double sigmaRecovery, double lambdaFailure)
        {

            double ItemIsIdle = 0;//Элемент в восстановлении
            double Itemserves = 0;//Элемент в отказе
            double valueOfWorkingStates = 0;
            double valueOfNonWorkingStates = 0;
            states[0][0] = 1;
            Itemserves = 1/ExponentialDistribution(myRandom, lambdaFailure);
            ItemIsIdle = Itemserves;
            resultsFailure.Add(Itemserves);
            for (double mainTime = 0; mainTime < timeSimulation;)
            {
                mainTime += 1;
                // valueOfWorkingStates = mainTime;               
                if (states[countOfStates][0] == 0)
                {
                    //valueOfWorkingStates = 0;
                    countOfStates++;
                    states[countOfStates][0] = 1;
                    states[countOfStates][1] = valueOfWorkingStates;

                }

                if (mainTime >= ItemIsIdle)
                {
                   
                    valueOfNonWorkingStates = NormalDistribution(muRecovery, sigmaRecovery);
                    valueOfWorkingStates = Itemserves;
                    states[countOfStates][1] = valueOfWorkingStates;
                    countOfStates++;
                    states[countOfStates][0] = 0;
                    states[countOfStates][1] = valueOfNonWorkingStates;
                    resultsRecovery.Add(valueOfNonWorkingStates);
                    Itemserves = 1/ExponentialDistribution(myRandom, lambdaFailure);
                    resultsFailure.Add(Itemserves);
                    ItemIsIdle += Itemserves;
                    mainTime += valueOfNonWorkingStates;
                }


            }

        }
        private void ModelingNormalExponential(List<List<double>> states, ref int countOfStates, List<double> resultsRecovery,
           List<double> resultsFailure, double muFailure, double sigmaFailure, double lambdaRecovery)
        {

            double ItemIsIdle = 0;//Элемент в восстановлении
            double Itemserves = 0;//Элемент в отказе
            double valueOfWorkingStates = 0;
            double valueOfNonWorkingStates = 0;
            states[0][0] = 1;
            Itemserves = NormalDistribution(muFailure, sigmaFailure);
            ItemIsIdle = Itemserves;
            resultsFailure.Add(Itemserves);
            for (double mainTime = 0; mainTime < timeSimulation;)
            {
                mainTime += 1;
                // valueOfWorkingStates = mainTime;               
                if (states[countOfStates][0] == 0)
                {
                    //valueOfWorkingStates = 0;
                    countOfStates++;
                    states[countOfStates][0] = 1;
                    states[countOfStates][1] = valueOfWorkingStates;

                }

                if (mainTime >= ItemIsIdle)
                {
                   
                    valueOfNonWorkingStates = ExponentialDistribution(myRandom, lambdaRecovery);
                    valueOfWorkingStates = Itemserves;
                    states[countOfStates][1] = valueOfWorkingStates;
                    countOfStates++;
                    states[countOfStates][0] = 0;
                    states[countOfStates][1] = valueOfNonWorkingStates;
                    resultsRecovery.Add(valueOfNonWorkingStates);
                    Itemserves = NormalDistribution(muFailure, sigmaFailure);
                    resultsFailure.Add(Itemserves);
                    ItemIsIdle += Itemserves;
                    mainTime += valueOfNonWorkingStates;
                }
            }

        }
        //среднее время наработки до отказа подумать над этим! Отказ совершает остановку работы, а восстановление - время восстановления до рабочего сост.
        private void ModelingNormalNormal(List<List<double>> states, ref int countOfStates, List<double> resultsRecovery,
           List<double> resultsFailure, double muFailure, double sigmaFailure, double muRecovery, double sigmaRecovery)
        {

            double ItemIsIdle = 0;//Элемент в восстановлении
            double Itemserves = 0;//Элемент в отказе
            double valueOfWorkingStates = 0;
            double valueOfNonWorkingStates = 0;
            states[0][0] = 1;
            Itemserves = NormalDistribution(muFailure, sigmaFailure);
            ItemIsIdle = Itemserves;
            resultsFailure.Add(Itemserves);
            double mainTime = 0;
            for (; mainTime < timeSimulation;)
            {
                mainTime += 1;               
               // valueOfWorkingStates = mainTime;               
                if (states[countOfStates][0] == 0)
                {
                    //valueOfWorkingStates = 0;
                    countOfStates++;
                    states[countOfStates][0] = 1;
                    states[countOfStates][1] = valueOfWorkingStates;
                    
                }

                if (mainTime >= ItemIsIdle)
                {                   
                    valueOfNonWorkingStates = NormalDistribution(muRecovery, sigmaRecovery);
                    valueOfWorkingStates = Itemserves;
                    states[countOfStates][1] = valueOfWorkingStates;
                    countOfStates++;
                    states[countOfStates][0] = 0;                    
                    states[countOfStates][1] = valueOfNonWorkingStates;
                    resultsRecovery.Add(valueOfNonWorkingStates);
                    Itemserves = NormalDistribution(muFailure, sigmaFailure);
                    resultsFailure.Add(Itemserves);
                    ItemIsIdle += Itemserves;
                    mainTime += valueOfNonWorkingStates;
                }
            }
        }
        private void ModelingExponentialExponential(List<List<double>> states, ref int countOfStates, List<double> resultsRecovery,
          List<double> resultsFailure, double lambdaFailure, double lambdaRecovery)
        {

            double ItemIsIdle = 0;//Элемент в восстановлении
            double Itemserves = 0;//Элемент в отказе
            double valueOfWorkingStates = 0;
            double valueOfNonWorkingStates = 0;
            states[0][0] = 1;
            Itemserves = ExponentialDistribution(myRandom, lambdaFailure);
            ItemIsIdle = Itemserves;
            resultsFailure.Add(Itemserves);
            for (double mainTime = 0; mainTime < timeSimulation;)
            {
                mainTime += 1;
                // valueOfWorkingStates = mainTime;               
                if (states[countOfStates][0] == 0)
                {
                    //valueOfWorkingStates = 0;
                    countOfStates++;
                    states[countOfStates][0] = 1;
                    states[countOfStates][1] = valueOfWorkingStates;

                }

                if (mainTime >= ItemIsIdle)
                {
                   
                    valueOfNonWorkingStates = ExponentialDistribution(myRandom, lambdaRecovery);
                    valueOfWorkingStates = Itemserves;
                    states[countOfStates][1] = valueOfWorkingStates;
                    countOfStates++;
                    states[countOfStates][0] = 0;
                    states[countOfStates][1] = valueOfNonWorkingStates;
                    resultsRecovery.Add(valueOfNonWorkingStates);
                    Itemserves = ExponentialDistribution(myRandom, lambdaFailure);
                    resultsFailure.Add(Itemserves);
                    ItemIsIdle += Itemserves;
                    mainTime += valueOfNonWorkingStates;
                }
            }
        }
        private void muRecoveryUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(","))) 
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
                
        }

        private void sigmaRecoveryUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(",")))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void lambdaRecoveryUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(",")))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void timeSumulationUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void clearGraph()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            countOfStates = 0;
            countOfAvailabilityFactor = 0;
            valueOfNonWorkingStates = 0;
            valueOfWorkingStates = 0;
            toolStripProgressBar1.Value = 0;
            countIteration = 0;
            countInfelicity = 0.01;
            availabilityFactor.Clear();
            states.Clear();

        }
        private void returnDefaultState()
        {
            clearGraph();
            timeSumulationUserBox.Text = "1000";
            muRecoveryUserBox.Text = "5";
            sigmaRecoveryUserBox.Text = "1";
            muFailureUserBox.Text = "100";
            sigmaFailureUserBox.Text = "5";
            CountIterationBox.Text = "100000";
            InfelicityUserBox.Text = "0,001";
            radioButtonTime.Checked = true;
            lambdaFailureUserBox.Text = null;
            lambdaRecoveryUserBox.Text = null;
            workLabel.Visible = false;
            toolStripProgressBar1.Visible = false;
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            returnDefaultState();
            zedGraphControl1.Refresh();
           
        }

        private void workLabel_TextChanged(object sender, EventArgs e)
        {
            toolStripMenuItem1.Enabled = true;
            workLabel.Refresh();
        }

        private void muRecoveryUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void sigmaRecoveryUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void lambdaRecoveryUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void timeSumulationUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==0 && comboBox2.SelectedIndex == 1)
            {
                sigmaFailureUserBox.Enabled = false;
                muFailureUserBox.Enabled = false;
                lambdaRecoveryUserBox.Enabled = false;
                sigmaRecoveryUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                lambdaFailureUserBox.Enabled = true;
                if (sigmaRecoveryUserBox.Text != "" && muRecoveryUserBox.Text != "" && lambdaFailureUserBox.Text != "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if(comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 0)
            {
                sigmaRecoveryUserBox.Enabled = false;
                muRecoveryUserBox.Enabled = false;
                lambdaFailureUserBox.Enabled = false;
                sigmaRecoveryUserBox.Enabled = false;
                muRecoveryUserBox.Enabled = false;
                lambdaFailureUserBox.Enabled = false;
                sigmaFailureUserBox.Enabled = true;
                muFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
                if (sigmaFailureUserBox.Text != "" && muFailureUserBox.Text != "" && lambdaRecoveryUserBox.Text!="")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 0)
            {
                lambdaFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
                muFailureUserBox.Enabled = false;
                muRecoveryUserBox.Enabled = false;
                sigmaFailureUserBox.Enabled = false;
                sigmaRecoveryUserBox.Enabled = false;
                if (lambdaRecoveryUserBox.Text != "" && lambdaFailureUserBox.Text!="")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 1)
            {
                lambdaFailureUserBox.Enabled = false;
                lambdaRecoveryUserBox.Enabled = false;
                muFailureUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                sigmaFailureUserBox.Enabled = true;
                sigmaRecoveryUserBox.Enabled = true;
                if (muRecoveryUserBox.Text != "" && sigmaRecoveryUserBox.Text != "" && muFailureUserBox.Text!="" && sigmaFailureUserBox.Text!= "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else
            {
                muFailureUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                sigmaFailureUserBox.Enabled = true;
                sigmaRecoveryUserBox.Enabled = true;
                lambdaFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 1)
            {
                sigmaFailureUserBox.Enabled = false;
                muFailureUserBox.Enabled = false;
                lambdaRecoveryUserBox.Enabled = false;
                sigmaRecoveryUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                lambdaFailureUserBox.Enabled = true;
                if (sigmaRecoveryUserBox.Text != "" && muRecoveryUserBox.Text != "" && lambdaFailureUserBox.Text != "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 0)
            {
                sigmaRecoveryUserBox.Enabled = false;
                muRecoveryUserBox.Enabled = false;
                lambdaFailureUserBox.Enabled = false;
                sigmaFailureUserBox.Enabled = true;
                muFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
                if (sigmaFailureUserBox.Text != "" && muFailureUserBox.Text != "" && lambdaRecoveryUserBox.Text != "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if (comboBox1.SelectedIndex == 0 && comboBox2.SelectedIndex == 0)
            {
                lambdaFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
                muFailureUserBox.Enabled = false;
                muRecoveryUserBox.Enabled = false;
                sigmaFailureUserBox.Enabled = false;
                sigmaRecoveryUserBox.Enabled = false;
                if (lambdaRecoveryUserBox.Text != "" && lambdaFailureUserBox.Text != "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else if (comboBox1.SelectedIndex == 1 && comboBox2.SelectedIndex == 1)
            {
                lambdaFailureUserBox.Enabled = false;
                lambdaRecoveryUserBox.Enabled = false;
                muFailureUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                sigmaFailureUserBox.Enabled = true;
                sigmaRecoveryUserBox.Enabled = true;
                if (muRecoveryUserBox.Text != "" && sigmaRecoveryUserBox.Text != "" && muFailureUserBox.Text != "" && sigmaFailureUserBox.Text != "")
                {
                    StartModelingBtn.Enabled = true;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                }
            }
            else
            {
                muFailureUserBox.Enabled = true;
                muRecoveryUserBox.Enabled = true;
                sigmaFailureUserBox.Enabled = true;
                sigmaRecoveryUserBox.Enabled = true;
                lambdaFailureUserBox.Enabled = true;
                lambdaRecoveryUserBox.Enabled = true;
            }
        }

        private void CountIterationBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b' )
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void CheckFields()
        {
            if (lambdaRecoveryUserBox.Enabled == false && lambdaFailureUserBox.Enabled == false)
            {
                if (muRecoveryUserBox.Text.Length > 0 && sigmaRecoveryUserBox.Text.Length > 0
                                && muFailureUserBox.Text.Length > 0 && sigmaFailureUserBox.Text.Length > 0 &&
                                timeSumulationUserBox.Text.Length > 0 && CountIterationBox.Text.Length>0 && InfelicityUserBox.Text.Length>0)
                {
                    StartModelingBtn.Enabled = true;
                    return;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                    return;
                }
            }
            if (lambdaFailureUserBox.Enabled == false && muRecoveryUserBox.Enabled == false && sigmaRecoveryUserBox.Enabled == false)
            {
                if (lambdaRecoveryUserBox.Text.Length > 0 && muFailureUserBox.Text.Length > 0 && sigmaFailureUserBox.Text.Length > 0
                    && timeSumulationUserBox.Text.Length > 0 && CountIterationBox.Text.Length > 0 && InfelicityUserBox.Text.Length > 0)
                {
                    StartModelingBtn.Enabled = true;
                    return;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                    return;
                }
            }
            if (lambdaRecoveryUserBox.Enabled == false && muFailureUserBox.Enabled == false && sigmaFailureUserBox.Enabled == false)
            {
                if (lambdaFailureUserBox.Text.Length > 0 && muRecoveryUserBox.Text.Length > 0 && sigmaRecoveryUserBox.Text.Length > 0
                    && timeSumulationUserBox.Text.Length > 0 && CountIterationBox.Text.Length > 0 && InfelicityUserBox.Text.Length > 0)
                {
                    StartModelingBtn.Enabled = true;
                    return;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                    return;
                }
            }
            if (muRecoveryUserBox.Enabled == false && muFailureUserBox.Enabled == false &&
                sigmaRecoveryUserBox.Enabled == false && sigmaFailureUserBox.Enabled == false)
            {
                if (lambdaRecoveryUserBox.Text.Length > 0 && lambdaFailureUserBox.Text.Length > 0 && 
                    timeSumulationUserBox.Text.Length>0 && CountIterationBox.Text.Length > 0 && InfelicityUserBox.Text.Length > 0) 
                {
                    StartModelingBtn.Enabled = true;
                    return;
                }
                else
                {
                    StartModelingBtn.Enabled = false;
                    return;
                }
            }
                
        }
        private void muFailureUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void lambdaFailureUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void sigmaFailureUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void muFailureUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)  | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(",")))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void lambdaFailureUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)  | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(",")))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void sigmaFailureUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | e.KeyChar == '\b' | (e.KeyChar == Convert.ToChar(",")))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void CountIterationBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void radioButtonTime_CheckedChanged(object sender, EventArgs e)
        {
            InfelicityUserBox.Enabled = false;
            timeSumulationUserBox.Enabled = true;
        }

        private void radioButtonInfelicity_CheckedChanged(object sender, EventArgs e)
        {
            timeSumulationUserBox.Enabled = false;
            InfelicityUserBox.Enabled = true;
        }

        private void InfelicityUserBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void InfelicityUserBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }
        private bool CheckLogic()
        {
            if (muFailureUserBox.Enabled == true && muRecoveryUserBox.Enabled == true && sigmaFailureUserBox.Enabled == true && sigmaRecoveryUserBox.Enabled == true)
            {
                if (muFailure >= timeSimulation || sigmaFailure >= timeSimulation || (sigmaFailure * sigmaFailure) > timeSimulation
                                || (muRecovery >= timeSimulation || sigmaRecovery >= timeSimulation || (sigmaRecovery * sigmaRecovery) > timeSimulation))
                {
                    MessageBox.Show("Ошибка! Обратите внимание на ваши параметры отказа и восстановления. Возможные ошибки:\n" +
                        "1) математическое ожидание больше общего времени моделирования;\n" +
                        "2) среднеквадратичное отклонение больше общего времени моделирования;\n" +
                        "3) среднеквадратичное отклонение больще математического ожидания.");
                    return false;
                }
            }
            else if (lambdaFailureUserBox.Enabled == true && lambdaRecoveryUserBox.Enabled == true)
            {
                if (lambdaFailure >= timeSimulation || lambdaRecovery >= timeSimulation)
                {
                    MessageBox.Show("Ошибка! Обратите внимание на ваши параметры отказа и восстановления. Возможные ошибки:\n" +
                                            "1) T среднее больше общего времени моделирования.\n");
                    return false;
                }

            }
            else if (lambdaFailureUserBox.Enabled == true && muRecoveryUserBox.Enabled == true && sigmaRecoveryUserBox.Enabled == true)
            {
                if (lambdaFailure >= timeSimulation || muRecovery >= timeSimulation || sigmaRecovery >= timeSimulation)
                {
                    MessageBox.Show("Ошибка! Обратите внимание на ваши параметры отказа и восстановления. Возможные ошибки:\n" +
                                           "1) математическое ожидание больше общего времени моделирования;\n" +
                                           "2) среднеквадратичное отклонение больше общего времени моделирования;\n" +
                                           "3) среднеквадратичное отклонение больще математического ожидания;\n" +
                                           "4) Т среднее больше общего времени моделирования.");
                    return false;
                }

            }
            else if (lambdaRecoveryUserBox.Enabled == true && muFailureUserBox.Enabled == true && sigmaFailureUserBox.Enabled == true)
            {
                if (lambdaRecovery >= timeSimulation || muFailure >= timeSimulation || sigmaFailure >= timeSimulation)
                {
                    MessageBox.Show("Ошибка! Обратите внимание на ваши параметры отказа и восстановления. Возможные ошибки:\n" +
                                           "1) математическое ожидание больше общего времени моделирования;\n" +
                                           "2) среднеквадратичное отклонение больше общего времени моделирования;\n" +
                                           "3) среднеквадратичное отклонение больще математического ожидания;\n" +
                                           "4) Т среднее больше общего времени моделирования.");
                    return false;
                }
                
            }
            return true;
        }
        //private void panel1_Paint(object sender, PaintEventArgs e)
        //{

        //}
        //private void label4_Click(object sender, EventArgs e)
        //{

        //}
    }
}
