using System;
using System.IO;
using System.Threading;

namespace Proje1._2
{
    static class Program
    {
        public static System.Double[] distance(double[] testveri, double[,] veriler) // Verilen banknot verisinin verisetiyle karşılaştırılıp uzaklık hesaplanması
        {
            Double[] distlist = new Double[veriler.GetLength(0)];
            for (int i = 0; i < veriler.GetLength(0); i++)
            {
                // iki vektör arası uzaklık formülü kullanılarak uzaklık hesaplanır ve bir diziye aktarılır.
                double distance = Math.Sqrt(Math.Pow((testveri[0] - veriler[i, 0]), 2) + Math.Pow((testveri[1] - veriler[i, 1]), 2) + Math.Pow(testveri[2] - veriler[i, 2], 2) + Math.Pow(testveri[3] - veriler[i, 3], 2));
                distlist[i] = distance;

            }
            return distlist;
        }

        public static void Sort(double[] uzaklık, double[,] veriler)//distance metodundan elde edilen uzaklık verilerine göre azdan çoğa sıralama
        {
            var itemMoved = false;
            do
            {
                itemMoved = false;
                for (int i = 0; i < uzaklık.GetLength(0) - 1; i++)
                {
                    if (uzaklık[i] > uzaklık[i + 1])// Bubble sort algoritması mantığı ile dizi elemanları ikişer ikişer karşılaştırılır ve sıralanır.
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var lowerValueveriler = veriler[i + 1, j];//Uzaklık dizisi sıralanırken bir yandan veriseti de sıralanır.
                            veriler[i + 1, j] = veriler[i, j];
                            veriler[i, j] = lowerValueveriler;
                        }

                        var lowerValue = uzaklık[i + 1];
                        uzaklık[i + 1] = uzaklık[i];
                        uzaklık[i] = lowerValue;

                        itemMoved = true;
                    }
                }
            } while (itemMoved);
        }
        public static int Classify(double[] distance, double[,] veriler, int k)// Elde edilen sıralı uzaklığa ve girilen k değerine göre banknotun sınıflandırılması
        {
            int tempbanknot;
            int falsecount = 0;
            int truecount = 0;
            Console.WriteLine("Varyans\t\tÇarpıklık\tBasıklık\tEntropi\t\tSınıf\t\tUzaklık");
            for (int i = 0; i < k; i++)
            {
                if (veriler[i, 4] == 0)//verisetinde en yakın k verinin doğruluk değerlerine göre yeni banknotun doğruluk değeri tahminlenir.
                    falsecount++;
                else
                    truecount++;
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(veriler[i, j] + "\t\t");
                }
                Console.WriteLine(distance[i]);

            }
            if (truecount > falsecount)
            {
                tempbanknot = 1;
            }
            else if (falsecount > truecount)
                tempbanknot = 0;
            else
            {
                tempbanknot = (int)veriler[0, 4];
            }
            if (tempbanknot == 1)
                Console.WriteLine("Özellikleri verilen banknot gerçektir");
            else
                Console.WriteLine("Özellikleri verilen banknot sahtedir");
            return tempbanknot;
        }
        public static int ClassifyBaşarı(double[] distance, double[,] veriler, int k)// Başarı oranı hesaplanırken Classify metodundaki gereksiz printler kullanılmasın diye yazılan ClassifyBaşarı metodu.
        {
            // İşlevi Classify metoduyla aynıdır. Classify metodundan başarı oranı hesaplamada lazım olmayan gereksiz printler çıkartılmıştır.
            int tempbanknot;
            int falsecount = 0;
            int truecount = 0;
            for (int i = 0; i < k; i++)
            {
                if (veriler[i, 4] == 0)
                    falsecount++;
                else
                    truecount++;
            }
            if (truecount > falsecount)
            {
                tempbanknot = 1;
            }
            else if (falsecount > truecount)
                tempbanknot = 0;
            else
            {
                tempbanknot = (int)veriler[0, 4];
            }
            return tempbanknot;
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("veriler.txt"); // Veriler Dosyasının satırlarının okunarak diziye atılması
            double[,] veriler = new double[lines.Length, 5];
            string[] satırlar = new string[5];
            int i = 0;
            double[,] verilerdegismeyen = new double[lines.Length, 5];
            double[,] verilerbaşarı = new double[lines.Length, 5];
            foreach (string line in lines)//Satırların virgüle göre ayrıştırılıp veriler matrisine atılması
            {
                satırlar = line.Split(',');
                for (int j = 0; j < 5; j++)
                {
                    veriler[i, j] = Convert.ToDouble(satırlar[j], System.Globalization.CultureInfo.InvariantCulture);
                    verilerdegismeyen[i, j] = veriler[i, j];
                    verilerbaşarı[i, j] = veriler[i, j];
                }
                i++;
            }

            bool devam = true;
            while (devam) // Kullanıcıdan istediği kadar veriyi girmesi istenir.
            {
                try
                {
                    double[] testveri = new double[4];
                    Console.WriteLine("Banknotun varyans değerini giriniz:");
                    testveri[0] = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    Console.WriteLine("Banknotun çarpıklık değerini giriniz:");
                    testveri[1] = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    Console.WriteLine("Banknotun basıklık değerini giriniz:");
                    testveri[2] = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    Console.WriteLine("Banknotun entropi değerini giriniz:");
                    testveri[3] = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                    Console.WriteLine("K değerini giriniz:");
                    int k = Convert.ToInt32(Console.ReadLine());
                    double[] uzaklık = distance(testveri, veriler); //Uzaklar alınarak diziye atılır.
                    Sort(uzaklık, veriler);//Verilen uzaklıklar ve ona göre veriler sıralanır.
                    int banknotsınıf = Classify(uzaklık, veriler, k);//Sıralanan diziden k ya göre veri çekilir ve sınıflandırma yapılır.
                    Console.WriteLine("Banknotun sınıfı= " + banknotsınıf);
                }
                catch (System.FormatException e)//Hata kontrolü
                {
                    Console.WriteLine("Hatalı değer girdiniz.Sayı girmeniz gerekiyor!!!");
                }
                while (true)//Kullanıcıya devam etmek isteyip istemediği sorulur
                {
                    Console.WriteLine("Devam etmek istiyor musunuz (e/h):");
                    string kontrol = Console.ReadLine();
                    if (kontrol == "e")
                    {
                        devam = true;
                        break;
                    }
                    else if (kontrol == "h")
                    {
                        devam = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("e veya h harfleri girilmelidir!");
                    }
                }
            }
            //buradan sonra başarı oranı hesaplamaya geçtik
            double[,] testverileri = new double[200, 5];//verisetinden doğruluk değeri 0 olan son 100 veriyi ve 1 olan son 100 veriyi tutmak için dizi açtık.
            for (int j = 662; j < 762; j++)// doğruluk değeri 0 olan son 100 verinin testverileri dizisine aktarımı.
            {
                for (int k = 0; k < 5; k++)
                {
                    testverileri[j - 662, k] = verilerbaşarı[j, k];
                }

            }
            for (int j = 1272; j < 1372; j++)//doğruluk değeri 1 olan son 100 verinin testverileri dizisine aktarımı.
            {
                for (int k = 0; k < 5; k++)
                {
                    testverileri[j - 1172, k] = verilerbaşarı[j, k];
                }
            }
            double[,] verilerbaşarıson = new double[1172, 5];// verisetinden aldığımız 200 verinin eksiltilmiş hali ile verisetinin yeni bir diziye aktarılması.
            for (int j = 0; j < 662; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    verilerbaşarıson[j, k] = verilerbaşarı[j, k];
                }
            }
            for (int j = 762; j < 1272; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    verilerbaşarıson[j - 100, k] = verilerbaşarı[j, k];
                }
            }
            double[] sınıfbaşarı = new double[testverileri.GetLength(0)];// verisetinden aldığımız 200 verinin tahminlenen doğruluk değerlerini içeren bir dizi 
            int a;
            while (true) //Hata kontrolü
            {
                try
                {
                    Console.WriteLine("Başarı oranı için K değerini giriniz:");
                    a = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (System.FormatException e)
                {
                    Console.WriteLine("Hatalı değer girdiniz.Lütfen sayı giriniz!");
                }
            }
            Console.WriteLine("Veriler yükleniyor lütfen bekleyiniz...");
            for (int j = 0; j < testverileri.GetLength(0); j++)
            {
                double[] testtemp = new double[5];
                for (int k = 0; k < 5; k++)
                {
                    testtemp[k] = testverileri[j, k];//testverileri dizisindeki her eleman veriseti ile karşılaştırılıp sınıflandırılır
                }
                double[] uzaklıkbaşarı = distance(testtemp, verilerbaşarıson);
                Sort(uzaklıkbaşarı, verilerbaşarıson);
                int başarıclassify = ClassifyBaşarı(uzaklıkbaşarı, verilerbaşarıson, a);
                sınıfbaşarı[j] = Convert.ToDouble(başarıclassify);
            }
            double[] gerçeksınıf = new double[testverileri.GetLength(0)];// burada ise verisetinden alınan 200 verinin gerçek doğruluk değerleri tutulur.
            for (int k = 0; k < gerçeksınıf.GetLength(0); k++)
                gerçeksınıf[k] = testverileri[k, 4];
            Console.WriteLine("Gerçek sınıf" + "\t\t" + "Tahminlenen sınıf");
            for (int k = 0; k < gerçeksınıf.GetLength(0); k++)
            {
                Console.WriteLine(gerçeksınıf[k] + "\t\t" + sınıfbaşarı[k]);// tahminlenen doğruluk verileriyle gerçek doğruluk verileri bastırılır.
            }

            double count = 0.0;
            for (int k = 0; k < gerçeksınıf.GetLength(0); k++)// tahminlenen doğruluk verilerinin kaç tanesinin gerçek verilerle uyuştuğu ölçülür.
            {
                if (gerçeksınıf[k] == sınıfbaşarı[k])
                {
                    count++;
                }

            }
            double başarıoranı = count / gerçeksınıf.GetLength(0) * 100;// ölçülen uyuşma sayısı ile programın başarı oranı hesaplanır.
            Console.WriteLine("Başarı oranı: %" + başarıoranı);
            Console.WriteLine("\n************\n");
            Console.WriteLine("Veriseti yükleniyor lütfen bekleyiniz...");
            Thread.Sleep(3000);
            Console.WriteLine("Bellekteki Veriseti: ");
            Console.WriteLine("Varyans\t\tÇarpıklık\tBasıklık\tEntropi\t\tSınıf");
            for (int j = 0; j < verilerdegismeyen.GetLength(0); j++)// Verisetinin ilk ve değiştirilmemiş hali yazdırılır.
            {
                for (int y = 0; y < 5; y++)
                {
                    Console.Write(verilerdegismeyen[j, y] + "\t\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}