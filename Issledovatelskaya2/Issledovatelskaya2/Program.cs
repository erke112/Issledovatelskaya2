using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Console;
using Extreme.Mathematics;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Issledovatelskaya2
{
    public class stageClass
    {
        public Decimal isp;
        public Decimal specificThrust;
        public Decimal massStart;
        public Decimal massFin;
        public Decimal deltaV;
        public Decimal thrust100;
        public Decimal timeToWork;
        public Decimal TWRstart;
        public Decimal TWRfin;
        public Decimal massflow100;
    }
    class Program
    {
        static void Main(string[] args)
        {
            const Decimal shag = 1M;
            Decimal time, timePred;
            Decimal x, xPred, y, yPred, vel, velPred, velX, velXPred, velY, velYPred, angleVelRad, gEar, gEarX, gEarY, angleGEarthRad, gMoo, gMooX, gMooY, angleGMooRad;
            Decimal accel, accelX, accelY, angleAccelRad, angleAccelDegr, thrustPercent, thrust, massflow, mass, massPred, timeBeforeBurnOut, TWR, fuelRemainPercent, deltaVel;
            Decimal heightEar, soundSpeed, airDensity, Cx, airForce, machNumber, Cx_withoutK, sectional;
            Decimal moonX, moonY, moonVX, moonVY, moonV, moonAngleV, moonGfromEar, moonGfromEarX, moonGfromEarY, moonXPred, moonYPred, moonVXPred, moonVYPred, moonGfromEarXPred, moonGfromEarYPred, moonAngleG;
            int stageNumber, stageKolvo;
            const Decimal massEarthG = 398600441580000;//гравитационный параметр
            const Decimal massMoonG = 4902800030000;
            const Decimal radiusEarth = 6378100;
            const Decimal radiusMoon = 1738140;
            stageClass[] stage = new stageClass[1];
            bool trigStageExit = false;
            StreamWriter writer = new StreamWriter(@"D:\Advanced Grapher\Untitled2.txt");


            /*
            Console.Write("Количество ступеней? = "); stageKolvo = int.Parse(Console.ReadLine());
            Array.Resize(ref stage, stageKolvo);

            for (int i = 0; i < stageKolvo; i++)
            {
                stage[i] = new stageClass();
            }

            for (int kolvo = 0; kolvo < stageKolvo; kolvo++)
            {
                do
                {


                    Console.WriteLine("Характеристики ступени " + (kolvo + 1));
                    Console.Write("Удельная тяга, сек = "); stage[kolvo].specificThrust = Decimal.Parse(Console.ReadLine());
                    Console.Write("Wet mass, кг = "); stage[kolvo].massStart = Decimal.Parse(Console.ReadLine());
                    Console.Write("Dry mass, кг = "); stage[kolvo].massFin = Decimal.Parse(Console.ReadLine());
                    Console.Write("Время работы при 100% тяги, сек = "); stage[kolvo].timeToWork = Decimal.Parse(Console.ReadLine());

                    stage[kolvo].isp = stage[kolvo].specificThrust * 9.8M;
                    stage[kolvo].deltaV = stage[kolvo].isp * Extreme.Mathematics.DecimalMath.Log(stage[kolvo].massStart / stage[kolvo].massFin);
                    stage[kolvo].massflow100 = (stage[kolvo].massStart - stage[kolvo].massFin) / stage[kolvo].timeToWork;
                    stage[kolvo].thrust100 = stage[kolvo].isp * stage[kolvo].massflow100;
                    stage[kolvo].TWRstart = stage[kolvo].thrust100 / (stage[kolvo].massStart * 9.8M);
                    stage[kolvo].TWRfin = stage[kolvo].thrust100 / (stage[kolvo].massFin * 9.8M);

                    Console.WriteLine(); Console.WriteLine(); Console.WriteLine();

                    Console.WriteLine($"Характеристики ступени {(kolvo + 1)}");
                    Console.WriteLine($"Удельная тяга  {stage[kolvo].specificThrust.ToString("0.000")} с    Isp  {stage[kolvo].isp.ToString("0.000")} м/с");
                    Console.WriteLine($"Масса начальная  {stage[kolvo].massStart.ToString("0.000")} кг      Масса сухая  {stage[kolvo].massFin.ToString("0.000")} кг");
                    Console.WriteLine($"Время работы при 100%  {stage[kolvo].timeToWork.ToString("0.000")} с");
                    Console.WriteLine($"Массовое потребление топлива  {stage[kolvo].massflow100.ToString("0.000")} кг/с        Тяга 100%  {stage[kolvo].thrust100.ToString("0.000")} H");
                    Console.WriteLine($"Характеристическая скорость  {stage[kolvo].deltaV.ToString("0.000")} м/с");
                    Console.WriteLine($"TWR wet  {stage[kolvo].TWRstart.ToString("0.000")} g       TWR dry  {stage[kolvo].TWRfin.ToString("0.000")} g");
                    Console.WriteLine();
                    Console.WriteLine("Подтверждаете ?   Yes - Перейти далее     No - задать заного");

                    switch (Console.ReadLine())
                    {
                        case ("Yes"):
                            trigStageExit = true; break;
                        default:
                            break;

                    }
                    Console.Clear();
                }
                while (trigStageExit == false);
            }
            */
            /*
           
            x = xPred + (velX * shag);
            y = yPred + (velY * shag);
            */
            timePred = 0; time = 0;
            moonX = 0; moonY = 405696000;
            moonAngleV = DecimalMath.Pi * 1.5M;//Начальные значения
            //moonV = 963.38744M;
            moonVXPred = -963.368300825M; moonVYPred = 0;
            moonGfromEarXPred = 0; moonGfromEarYPred = 0;
            moonXPred = 0; moonYPred = 405696000;
            moonVX = 0; moonVY = 0;

            while (true)
            {
                int i = 0;
                for (i = 0; i < 1000; i++)
                {
                    time = timePred + shag;
                    moonVX = moonVXPred + (moonGfromEarXPred * shag);
                    moonVY = moonVYPred + (moonGfromEarYPred * shag);
                    moonX = moonXPred + (moonVX * shag);
                    moonY = moonYPred + (moonVY * shag);
                    moonGfromEar = massEarthG /
                        (DecimalMath.Pow(moonX, 2) + DecimalMath.Pow(moonY, 2));

                    Decimal k = new decimal();
                    if (moonY > 0)
                    {
                        k = 1;
                    }
                    if ((moonX > 0) && (moonY < 0))         //проверка четверти для moonAngleG
                    {
                        k = 2;
                    }
                    if ((moonX < 0) && (moonY < 0))
                    {
                        k = 0;
                    }
                    moonAngleG = DecimalMath.Atan(moonX / moonY) + DecimalMath.Pi * k;
                    moonGfromEarX = moonGfromEar * DecimalMath.Sin(moonAngleG);
                    moonGfromEarY = moonGfromEar * DecimalMath.Cos(moonAngleG);

                    if (moonVX < 0) { k = 1.5M; } else { k = 0.5M; }
                    //moonAngleV = -1 * DecimalMath.Atan(moonVY / moonVX) + (DecimalMath.Pi * k);



                    //
                    //перенос настоящих значений полученных выше в бд
                    //

                    timePred = time;
                    moonXPred = moonX; moonYPred = moonY;
                    moonVXPred = moonVX; moonVYPred = moonVY;
                    moonGfromEarXPred = moonGfromEarX; moonGfromEarYPred = moonGfromEarY;
                }

                Console.WriteLine($"{time.ToString("0.00")}  {moonX.ToString("0.000")}   {moonY.ToString("0.000")}   {moonVX.ToString("0.000")}    {moonVY.ToString("0.000")}  ");
                writer.WriteLine($" {moonX}  {moonY}");
                if (time > 10000000) { writer.Close(); Thread.Sleep(1000 * 60 * 60); }

            }
            Console.WriteLine(moonGfromEarX);
            Console.ReadLine();
        }
    }
}
