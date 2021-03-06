﻿using System;
using MathNet.Numerics.Distributions;
using System.IO;

namespace D2D_simulation
{
    class Program
    {
        static void Main()
        {

            //B1: Xay dung he thong: tao cac UE, FBS, MBS
            Create_D2Dsystem();
            //B2: Khoi tao cac su kien ban dau truoc khi mo phong
            Create_eventList();
            //B3: Tao fuction chay cac su kien trong qua trình mo phong
            Excuting_Event();
            //B4: Thu thap du lieu de phan tich
            Output();
            Console.WriteLine("Complete");
        }
        public static void Create_D2Dsystem()
        {
            //B1-1: Tao MBS
            MBS new_MBS = new MBS(400, 400, 0)
            {
                Id_MBS = 1
            };
            Input.List_MBS.Add(new_MBS);
            //Khoi tao channel add vao listchanel cua MBS
            for (int i = 0; i < Input.NumberofChannel; i++)
            {
                Channel channel = new Channel();
                channel.channel_id = i + 1;
                Input.List_MBS[0].ListChannel.Add(channel);
            }
            //Lay position tu file
            string path1 = Input.path + "\\list_device_in_system_" + Input.NumberofSBS + ".txt";
            if (System.IO.File.Exists(@path1) == true)
            {
                FileStream fs = new FileStream(path1, FileMode.Open);
                StreamReader read1 = new StreamReader(fs);
                int NumberOfMUE = Convert.ToInt32(read1.ReadLine());
                for (int i = 0; i < NumberOfMUE; i++)
                {
                    UE newDv = new UE();
                    int X = Convert.ToInt32(read1.ReadLine());
                    int Y = Convert.ToInt32(read1.ReadLine());
                    Position newpoint = new Position(X, Y, 0);
                    newDv.UE_Id = ++Input.UE_Id;
                    newDv.PowerTr = Input.Max_PowerTr;
                    newDv.Position = newpoint;
                    Input.List_MBS[0].ListUE.Add(newDv);
                }
                int NumberOfSBS = Convert.ToInt32(read1.ReadLine());
                for (int i=0; i < NumberOfSBS; i++)
                {
                    SBS newSBS = new SBS();
                    newSBS.Id_SBS = i + 1;
                    int X = Convert.ToInt32(read1.ReadLine());
                    int Y = Convert.ToInt32(read1.ReadLine());
                    Position newpoint = new Position(X, Y, 0);
                    newSBS.Position = newpoint;
                    Input.List_SBS.Add(newSBS);
                    UE new_SUE = new UE(X, Y, 0);
                    new_SUE.PowerTr = Input.SUE_Max_PowerTr;
                    new_SUE.UE_Id = ++Input.UE_Id;
                    newSBS.ListUE.Add(new_SUE);
                }
                int NumberOfPair = Convert.ToInt32(read1.ReadLine());
                for (int i = 0; i < NumberOfPair; i++)
                {
                    Pair pair = new Pair();
                    int X = Convert.ToInt32(read1.ReadLine());
                    int Y = Convert.ToInt32(read1.ReadLine());
                    UE t_UE = new UE(X, Y, 0);
                    t_UE.UE_Id = ++Input.UE_Id;
                    int X2 = Convert.ToInt32(read1.ReadLine());
                    int Y2 = Convert.ToInt32(read1.ReadLine());
                    UE r_UE = new UE(X2, Y2, 0);
                    t_UE.UE_Id = ++Input.UE_Id;
                    pair.UE_r = r_UE;
                    pair.UE_t = t_UE;
                    t_UE.PowerTr = Input.D2D_Max_PowerTr;
                    r_UE.PowerTr = Input.D2D_Max_PowerTr;
                    Input.List_MBS[0].ListPair.Add(pair);
                }
                read1.Close();
            }
            /*else
            {
                //Khoi tao vi tri M-UE
                FileStream fs = new FileStream(path1, FileMode.Create);
                StreamWriter sw1 = new StreamWriter(fs);
                sw1.WriteLine(Input.NumberofM_UE);
                for (int i = 0; i < Input.NumberofM_UE; i++)
                {
                    UE newUE = new UE();
                    newUE.UE_Id = ++Input.UE_Id;
                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                    int x = 0, y = 0;
                    {
                        if (i % 4 == 0)
                        {
                            x = rd.Next(0, 400);
                            y = rd.Next(0, 400);
                        }
                        else if (i % 4 == 1)
                        {
                            x = rd.Next(0, 400);
                            y = rd.Next(400, 800);
                        }
                        else if (i % 4 == 2)
                        {
                            x = rd.Next(400, 800);
                            y = rd.Next(0, 400);
                        }
                        else if (i % 4 == 3)
                        {
                            x = rd.Next(400, 800);
                            y = rd.Next(400, 800);
                        }
                    }
                    Position position = new Position(x, y, 0);
                    newUE.Position = position;
                    newUE.PowerTr = Input.Max_PowerTr;
                    Input.List_MBS[0].ListUE.Add(newUE);
                    sw1.WriteLine(x);
                    sw1.WriteLine(y);
                }
                //Khoi tao SBS chia thanh 4 vung phan bo deu va S_UE, gan Power cho UE
                sw1.WriteLine(Input.NumberofSBS);
                for (int i = 0; i < Input.NumberofSBS; i++)
                {
                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                    int x = 0, y = 0;
                    {
                        if (i % 4 == 0)
                        {
                            x = rd.Next(0, 400);
                            y = rd.Next(0, 400);
                        }
                        else if (i % 4 == 1)
                        {
                            x = rd.Next(0, 400);
                            y = rd.Next(400, 800);
                        }
                        else if (i % 4 == 2)
                        {
                            x = rd.Next(400, 800);
                            y = rd.Next(0, 400);
                        }
                        else if (i % 4 == 3)
                        {
                            x = rd.Next(400, 800);
                            y = rd.Next(400, 800);
                        }
                    }
                    SBS new_SBS = new SBS(x, y, 0);
                    UE new_SUE = new UE(x + rd.Next(-10, 10), y + rd.Next(-10, 10), 0);
                    new_SUE.PowerTr = Input.SUE_Max_PowerTr;
                    new_SBS.Id_SBS = i + 1;
                    new_SUE.UE_Id = ++Input.UE_Id;
                    Input.List_SBS.Add(new_SBS);
                    new_SBS.ListUE.Add(new_SUE);
                    sw1.WriteLine(x);
                    sw1.WriteLine(y);
                }
                //Khoi tao D2DPair
                sw1.WriteLine(Input.NumberofPair);
                for (int i = 0; i < Input.NumberofPair; i++)
                {
                    Pair pair = new Pair();

                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                    int x = 0, y = 0;
                    {
                        if (i % 4 == 0)
                        {
                            x = rd.Next(0 + Input.D2DCover, 400);
                            y = rd.Next(0 + Input.D2DCover, 400);
                        }
                        else if (i % 4 == 1)
                        {
                            x = rd.Next(0 + Input.D2DCover, 400);
                            y = rd.Next(400, 800 - Input.D2DCover);
                        }
                        else if (i % 4 == 2)
                        {
                            x = rd.Next(400, 800 - Input.D2DCover);
                            y = rd.Next(0 + Input.D2DCover, 200);
                        }
                        else if (i % 4 == 3)
                        {
                            x = rd.Next(400, 800 - Input.D2DCover);
                            y = rd.Next(400, 800 - Input.D2DCover);
                        }
                    }
                    UE t_UE = new UE(x, y, 0);
                    t_UE.UE_Id = ++Input.UE_Id;
                    UE r_UE = new UE(x + Input.D2DCover, y + Input.D2DCover, 0);
                    t_UE.PowerTr = Input.D2D_Max_PowerTr;
                    r_UE.PowerTr = Input.D2D_Max_PowerTr;
                    pair.UE_r = r_UE;
                    pair.UE_t = t_UE;
                    Input.List_MBS[0].ListPair.Add(pair);
                    sw1.WriteLine(x);
                    sw1.WriteLine(y);
                    sw1.WriteLine(x + Input.D2DCover);
                    sw1.WriteLine(y + Input.D2DCover);
                }
                sw1.Close();
            }*/

        }
        public static void Create_eventList() { }
        public static void Excuting_Event() {
            //Khoi tao request cap kenh cho M-UE
            for (int i = 0; i < Input.List_MBS[0].ListUE.Count; i++)
            {
                Request new_Request = new Request(Input.List_MBS[0].ListUE[i]);
                Input.List_MBS[0].ListRequest.Add(new_Request);
            }
            //MBS xu li cap kenh cho M-UE
            RequestProcess.processRequest();
            //Tinh Pmax tren moi kenh
            for (int i = 0; i < Input.List_MBS[0].ListUE.Count; i++)
            {
                double linkgain, pathloss, shadowing;
                var lognormal = new LogNormal(0, 1);
                shadowing = lognormal.Sample();
                pathloss = Input.List_MBS[0].ListUE[i].PassLoss_Indoor(Input.List_MBS[0].Position);

                //pathloss = 127 + 30 * Math.Log10(Input.MBSCover * Math.Pow(10, -3)); ;

                linkgain = pathloss + shadowing;
                Input.List_MBS[0].ListUE[i].Shadowing = shadowing;
                Input.List_MBS[0].ListUE[i].Linkgain = Input.List_MBS[0].ListUE[i].PassLoss_Indoor(Input.List_MBS[0].Position) + shadowing;
                //Console.WriteLine("Linkgain is MUE: " + linkgain);*/
                Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].MaxPower = Input.Max_PowerTr - linkgain - Input.SINR_threshold;
                //Console.WriteLine("Total Interference permit: " + Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].MaxPower);

            }
            /*//S-UE chon ngau nhien s kenh
            for (int i = 0; i < Input.List_SBS.Count; i++)
            {
                for (int j = 0; j < Input.s; j++)
                {
                    double linkgain;
                    double pathloss, shadowing;
                    pathloss = Input.List_SBS[i].ListUE[0].PassLoss(Input.List_MBS[0].Position);
                    var lognormal = new LogNormal(0, 1);
                    shadowing = lognormal.Sample();
                    linkgain = pathloss + shadowing;
                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                    int channel_id = rd.Next(1, Input.NumberofChannel);
                    UE ue = new UE();
                    //Check id da dk lay hay chua
                    if (Input.List_SBS[i].ListUE[0].ListChannel_Id == null || Input.List_SBS[i].ListUE[0].ListChannel_Id.Contains(channel_id) == false)
                    {
                        Input.List_SBS[i].ListUE[0].Linkgain = linkgain;
                        ue.UE_Id = Input.List_SBS[i].ListUE[0].UE_Id;
                        ue.Position = Input.List_SBS[i].ListUE[0].Position;
                        ue.Linkgain = linkgain;
                        Input.List_SBS[i].ListUE[0].ListChannel_Id.Add(channel_id);
                        Input.List_MBS[0].ListChannel[channel_id - 1].ListSUE.Add(ue);
                    }
                    else
                    {
                        //Neu lay r thi chon lai id khac
                        while (Input.List_SBS[i].ListUE[0].ListChannel_Id.Contains(channel_id) == true)
                        {
                            channel_id = rd.Next(1, Input.NumberofChannel);
                        }
                        Input.List_SBS[i].ListUE[0].Linkgain = linkgain;
                        ue.UE_Id = Input.List_SBS[i].ListUE[0].UE_Id;
                        ue.Position = Input.List_SBS[i].ListUE[0].Position;
                        ue.Linkgain = linkgain;
                        Input.List_SBS[i].ListUE[0].ListChannel_Id.Add(channel_id);
                        Input.List_MBS[0].ListChannel[channel_id - 1].ListSUE.Add(ue);
                    }
                }
            }*/
            //Cap kenh moi Pair
            //Cap theo linkgain
            if (Input.Type == 1)
            {
                for (int i = 0; i < Input.List_MBS[0].ListPair.Count; i++)
                {
                    //Tinh Linkgain cua UE tren moi kenh
                    for (int j = 0; j < Input.List_MBS[0].ListChannel.Count; j++)
                    {
                        double linkgain;
                        double pathloss, shadowing;
                        pathloss = Input.List_MBS[0].ListPair[i].UE_t.PassLoss_Outdoor(Input.List_MBS[0].ListPair[i].UE_r.Position);
                        var lognormal = new LogNormal(0, 1);
                        shadowing = lognormal.Sample();
                        linkgain = pathloss + shadowing;
                        Channel channel = new Channel();
                        channel.channel_id = Input.List_MBS[0].ListChannel[j].channel_id;
                        channel.LinkGain = linkgain;
                        channel.Shadowing = shadowing;
                        //Add vao list xong xoa sau
                        Input.List_MBS[0].ListPair[i].UE_t.List_channel.Add(channel);
                    }

                    //Chon r kenh co linkgain cao nhat
                    //Sort lon dan linkgain
                    Input.List_MBS[0].ListPair[i].UE_t.quickSort(Input.List_MBS[0].ListPair[i].UE_t.List_channel, 0, Input.NumberofChannel - 1);
                    int numberofchannel_UE = Input.List_MBS[0].ListPair[i].UE_t.List_channel.Count;
                    //delete/chose r channel higher linkgain
                    for (int j = 0; j < numberofchannel_UE - Input.r; j++)
                    {
                        Input.List_MBS[0].ListPair[i].UE_t.List_channel.RemoveAt(0);
                    }

                    for (int j = 0; j < Input.List_MBS[0].ListPair[i].UE_t.List_channel.Count; j++)
                    {
                        UE ue = new UE();
                        ue.UE_Id = Input.List_MBS[0].ListPair[i].UE_t.UE_Id;
                        ue.Position = Input.List_MBS[0].ListPair[i].UE_t.Position;
                        // ue.Shadowing = Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].Shadowing;
                        int indexChannel = Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].channel_id - 1;
                        Input.List_MBS[0].ListChannel[indexChannel].List_device.Add(ue);
                    }
                    Console.WriteLine("*********************************************************");
                }
            }
            else
            {
                int IndexChannel = 0;
                for (int i = 0; i < Input.List_MBS[0].ListPair.Count; i++)
                {
                    if (IndexChannel == Input.NumberofChannel)
                        IndexChannel = 0;
                    //Tinh Linkgain cua UE tren moi kenh
                    double linkgain;
                    double pathloss, shadowing;
                    pathloss = Input.List_MBS[0].ListPair[i].UE_t.PassLoss_Outdoor(Input.List_MBS[0].ListPair[i].UE_r.Position);
                    var lognormal = new LogNormal(0, 1);
                    shadowing = lognormal.Sample();
                    linkgain = pathloss + shadowing;
                    Channel channel = new Channel();
                    channel.channel_id = IndexChannel + 1;
                    channel.LinkGain = linkgain;
                    channel.Shadowing = shadowing;
                    //Add vao list xong xoa sau
                    Input.List_MBS[0].ListPair[i].UE_t.List_channel.Add(channel);
                    IndexChannel++;
                    for (int j = 0; j < Input.List_MBS[0].ListPair[i].UE_t.List_channel.Count; j++)
                    {
                        UE ue = new UE();
                        ue.UE_Id = Input.List_MBS[0].ListPair[i].UE_t.UE_Id;
                        ue.Position = Input.List_MBS[0].ListPair[i].UE_t.Position;
                        // ue.Shadowing = Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].Shadowing;
                        int indexChannel = Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].channel_id - 1;
                        Input.List_MBS[0].ListChannel[indexChannel].List_device.Add(ue);
                    }
                    Console.WriteLine("*********************************************************");
                }
            }
            //Gioi han cong suat phat tren moi kenh doi voi cac device va SUE
            int sum = 0;
            for (int i = 0; i < Input.List_MBS[0].ListChannel.Count; i++)
            {
                double ratio = Input.proportion_Allow_Power;
                double PU_max = Input.List_MBS[0].ListChannel[i].MaxPower;
                int N_d2d = Input.List_MBS[0].ListChannel[i].List_device.Count;
                int N_sue = Input.List_MBS[0].ListChannel[i].ListSUE.Count;
                sum += N_d2d;
                if (N_d2d != 0)
                {
                    Input.List_MBS[0].ListChannel[i].D2D_MaxPower = (10 * Math.Log10(ratio) + PU_max) - (10 * Math.Log10(N_d2d));
                    Console.WriteLine("Interference each device=" + Input.List_MBS[0].ListChannel[i].D2D_MaxPower);
                }
                if (N_sue != 0)
                {
                    Input.List_MBS[0].ListChannel[i].SUE_MaxPower = (10 * Math.Log10(1 - ratio) + PU_max) - (10 * Math.Log10(N_sue));
                    Console.WriteLine("Interference each SUE=" + Input.List_MBS[0].ListChannel[i].SUE_MaxPower);
                }
            }
            Console.WriteLine("Sum device=" + sum);
            //UE dieu chinh cong suat phat 
            for (int i = 0; i < Input.List_MBS[0].ListChannel.Count; i++)
            {
                //Cho D2D
                if (Input.List_MBS[0].ListChannel[i].List_device.Count > 0)
                {
                    for (int j = 0; j < Input.List_MBS[0].ListChannel[i].List_device.Count; j++)
                    {
                        double linkgain, Power_cal;
                        double pathloss, shadowing;
                        var lognormal = new LogNormal(0, 1);
                        shadowing = lognormal.Sample();
                        pathloss = Input.List_MBS[0].ListChannel[i].List_device[j].PassLoss_Outdoor(Input.List_MBS[0].Position);
                        linkgain = pathloss + shadowing;
                        Input.List_MBS[0].ListChannel[i].List_device[j].Linkgain = linkgain;
                        Power_cal = (Input.List_MBS[0].ListChannel[i].D2D_MaxPower + linkgain);
                        Console.WriteLine("Linkgain from device to MBS=" + pathloss);
                        Input.List_MBS[0].ListChannel[i].List_device[j].PowerTr = Math.Min(Input.D2D_Max_PowerTr, (Power_cal));
                    }
                }
                //Cho SUE
                for (int k = 0; k < Input.List_MBS[0].ListChannel[i].ListSUE.Count; k++)
                {
                    double linkgain, Power_cal;
                    linkgain = Input.List_MBS[0].ListChannel[i].ListSUE[k].Linkgain;
                    Power_cal = Input.List_MBS[0].ListChannel[i].SUE_MaxPower + linkgain;
                    Console.WriteLine("Linkgain from SUE to MBS=" + Input.List_MBS[0].ListChannel[i].ListSUE[k].PassLoss_Outdoor(Input.List_MBS[0].Position));
                    Input.List_MBS[0].ListChannel[i].ListSUE[k].PowerTr = Math.Min(Input.SUE_Max_PowerTr, (Power_cal));
                }
            }
        }
        public static void Output()
        {
            //Tinh SINR tung M-UE
            double tPutMUE = 0;
            string path10 = Input.path + "\\MUEthroughput_each_device_Paper_" + Input.NumberofSBS + ".txt";
            FileStream fs10 = new FileStream(path10, FileMode.Create);
            StreamWriter sw10 = new StreamWriter(fs10);
            string distanceDevice = Input.path + "\\Distance_Device_reuse_channel_Paper_" + Input.NumberofSBS + ".txt";
            FileStream dtDv = new FileStream(distanceDevice, FileMode.Create);
            StreamWriter DistanceDevice = new StreamWriter(dtDv);
            Console.WriteLine("MUE");
           // double devicecollision = 0;

            for (int i = 0; i < Input.List_MBS[0].ListUE.Count; i++)
            {
                Console.WriteLine("*********************************");
                Console.WriteLine("MUE_" + Input.List_MBS[0].ListUE[i].UE_Id);
                int channel_id = Input.List_MBS[0].ListUE[i].channel_id - 1;
                double I_total = 0;
                double linkgain, pathloss, shadowing;
                var lognormal = new LogNormal(0, 1);
                shadowing = lognormal.Sample();
                linkgain = Input.List_MBS[0].ListUE[i].PassLoss_Indoor(Input.List_MBS[0].Position) + shadowing;

                double P_r = Input.List_MBS[0].ListUE[i].PowerTr - linkgain;
                double SINR;
                double M_Capacity = 0;
                //Xet SUE
                for (int j = 0; j < Input.List_MBS[0].ListChannel[channel_id].ListSUE.Count; j++)
                {
                    UE ue_t = Input.List_MBS[0].ListChannel[channel_id].ListSUE[j];
                    double Linkgain = ue_t.Linkgain;
                    I_total += Math.Pow(10, (ue_t.PowerTr / 10) - (Linkgain / 10));
                    Console.WriteLine("DeviceSUE " + ue_t.UE_Id + " has power=" + ue_t.PowerTr);
                }
                //Xet D2D
                for (int j = 0; j < Input.List_MBS[0].ListChannel[channel_id].List_device.Count; j++)
                {
                    UE ue_t = Input.List_MBS[0].ListChannel[channel_id].List_device[j];
                    double Linkgain = ue_t.Linkgain;
                    I_total += Math.Pow(10, (ue_t.PowerTr / 10) - (Linkgain / 10));
                    Console.WriteLine("DevicePair " + ue_t.UE_Id + " has power=" + ue_t.PowerTr);
                   // devicecollision += 1;
                }
                if (I_total != 0)
                {
                    I_total = 10 * Math.Log10(I_total);
                    SINR = P_r - I_total;
                }
                else
                {
                    SINR = P_r - Input.N0;
                }
                //Capacity
                {

                    Console.WriteLine("SINR=" + SINR);
                    double C;
                    if (SINR < 0.5 && SINR > Input.SINR_outage)
                        C = 0.25 * Input.B;
                    else if (SINR > 0.5 && SINR < 3.5)
                        C = 0.5 * Input.B;
                    else if (SINR > 3.5 && SINR < 6.5)
                        C = 1 * Input.B;
                    else if (SINR > 6.5 && SINR < 9)
                        C = 1.5 * Input.B;
                    else if (SINR > 9 && SINR < 12.5)
                        C = 2 * Input.B;
                    else if (SINR > 12.5 && SINR < 14.5)
                        C = 3 * Input.B;
                    else if (SINR > 14.5 && SINR < 16.5)
                        C = 3 * Input.B;
                    else if (SINR > 16.5 && SINR < 18.5)
                        C = 4 * Input.B;
                    else if (SINR >= 18.5)
                        C = 4.5 * Input.B;
                    else
                        C = 0;
                    M_Capacity += C;
                }
                sw10.WriteLine(M_Capacity.ToString().Replace(",", "."));
                Input.List_MBS[0].ListUE[i].SINR = SINR;
                tPutMUE += M_Capacity;
            }
           // Console.WriteLine("So device trung kenh: " + devicecollision);
            sw10.Close();

            //Tinh SINR tung pair
            Console.WriteLine("SINR pair--------------------------");
            double throughput = 0;
            double maxpower = 0;
            string path11 = Input.path + "\\D2DSINR_each_device_Paper_" + Input.NumberofSBS + ".txt";
            FileStream fs11 = new FileStream(path11, FileMode.Create);
            StreamWriter sw11 = new StreamWriter(fs11);
            string path111 = Input.path + "\\D2Dthroughput_each_device_Paper_" + Input.NumberofSBS + ".txt";
            FileStream fs111 = new FileStream(path111, FileMode.Create);
            StreamWriter sw111 = new StreamWriter(fs111);
            for (int i = 0; i < Input.List_MBS[0].ListPair.Count; i++)
            {
                UE ue_t = Input.List_MBS[0].ListPair[i].UE_t;
                UE ue_r = Input.List_MBS[0].ListPair[i].UE_r;
               // Console.WriteLine("Device" + ue_t.UE_Id);
                double Capacity = 0;
                for (int j = 0; j < ue_t.List_channel.Count; j++)
                {
                    double P_r;
                    double I_total = 0;
                    double SINR, C;
                    Channel channel = Input.List_MBS[0].ListChannel[ue_t.List_channel[j].channel_id - 1];
                    //Tim ue trong list_device
                    int index = 0;
                    while (channel.List_device[index].UE_Id != ue_t.UE_Id)
                    {
                        index++;
                    }
                    P_r = channel.List_device[index].PowerTr - ue_t.List_channel[j].LinkGain;
                    for (int k = 0; k < channel.ListMUE.Count; k++)
                    {
                        double pathloss = channel.ListMUE[k].PassLoss_Outdoor(ue_r.Position);
                        var lognormal = new LogNormal(0, 1);
                        double shadowing = lognormal.Sample();
                        double linkgain = pathloss + shadowing;
                        I_total += Math.Pow(10, (channel.ListMUE[k].PowerTr / 10) - (linkgain / 10));
                    }
                    for (int k = 0; k < channel.List_device.Count; k++)
                    {
                        double pathloss = channel.List_device[k].PassLoss_Outdoor(ue_r.Position);
                        var lognormal = new LogNormal(0, 1);
                        double shadowing = lognormal.Sample();
                        double linkgain = pathloss + shadowing;
                        if (channel.List_device[k].UE_Id != ue_t.UE_Id)
                            I_total += Math.Pow(10, (channel.List_device[k].PowerTr / 10) - (linkgain / 10));
                        DistanceDevice.WriteLine(ue_t.Position.distance(channel.List_device[k].Position, ue_r.Position).ToString().Replace(",", "."));

                    }
                    for (int k = 0; k < channel.ListSUE.Count; k++)
                    {
                        double pathloss = channel.ListSUE[k].PassLoss_Outdoor(ue_r.Position);
                        var lognormal = new LogNormal(0, 1);
                        double shadowing = lognormal.Sample();
                        double linkgain = pathloss + shadowing;
                        I_total += Math.Pow(10, (channel.ListSUE[k].PowerTr / 10) - (linkgain / 10));
                    }

                    if (I_total != 0)
                    {
                        I_total = 10 * Math.Log10(I_total);
                        SINR = P_r - I_total;
                        //Console.WriteLine("=> SINR_" + ue_r.UE_Id + " = " + SINR + "   (dB)");

                    }
                    else
                    {
                        SINR = P_r - Input.N0;
                    }
                    {
                        if (SINR < 0.5 && SINR > Input.SINR_outage)
                            C = 0.25 * Input.B;
                        else if (SINR > 0.5 && SINR < 3.5)
                            C = 0.5 * Input.B;
                        else if (SINR > 3.5 && SINR < 6.5)
                            C = 1 * Input.B;
                        else if (SINR > 6.5 && SINR < 9)
                            C = 1.5 * Input.B;
                        else if (SINR > 9 && SINR < 12.5)
                            C = 2 * Input.B;
                        else if (SINR > 12.5 && SINR < 14.5)
                            C = 3 * Input.B;
                        else if (SINR > 14.5 && SINR < 16.5)
                            C = 3 * Input.B;
                        else if (SINR > 16.5 && SINR < 18.5)
                            C = 4 * Input.B;
                        else if (SINR >= 18.5)
                            C = 4.5 * Input.B;
                        else
                            C = 0;
                    }
                    //Tinh Capacity cua Device
                    Capacity += C;
                    sw111.WriteLine(Capacity.ToString().Replace(",", "."));
                    sw11.WriteLine(SINR.ToString().Replace(",", "."));
                }
               // Console.WriteLine("Capacity=" + Capacity);
                throughput += Capacity;
            }
             sw11.Close();
            sw111.Close();
            DistanceDevice.Close();
            /*double powerSUE = 0;
            for (int i = 0; i < Input.List_SBS.Count; i++)
            {

                powerSUE += Input.List_SBS[i].ListUE[0].PowerTr;
            }
            Console.WriteLine("MAX POWER D2D = " + maxpower);
            Console.WriteLine("MAX POWER SUE = " + powerSUE);*/
            Console.WriteLine("Throughput of MUE=" + tPutMUE);
            Console.WriteLine("throughputPair=" + throughput);
            Input.List_MBS[0].ListTP.Add(throughput);
            //Check so luong device trong channel
            double totaldevice = 0;
            for (int i = 0; i < Input.List_MBS[0].ListChannel.Count; i++)
            {
                double sum = Input.List_MBS[0].ListChannel[i].List_device.Count + Input.List_MBS[0].ListChannel[i].ListSUE.Count;
             //   Console.WriteLine("Channel_" + i + "has " + sum);
                totaldevice += sum;
            }
            Console.WriteLine("Total device=" + totaldevice);
            Console.WriteLine("CENTRALIZE");
            /*    Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("Total D2D_pair = " + Input.List_Pair.Count);
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("L = 127 + 30*Log(d*10^-3) (dB) (distance: met)");
                Console.WriteLine("Total interference = (P_tr1/L1) + (P_tr2/L2) + ... + (P_tr10/L10)" +
                 "= 10^[(P_tr1(dBm)-L1(dB))/10] + 10^[(P_tr2(dBm)-L2(dB))/10] + ... + 10^[(P_tr10(dBm)-L10(dB))/10] ");
                Console.WriteLine("SINR(dB) = (P_tr-L)(dBm) - 10log_10_(I)(dB)");
                Console.WriteLine("------------------------------------------------------");*/
            /*    for (int i = 0; i < Input.List_MBS[0].ListUE.Count; i++)
                {
                    Console.WriteLine("UE" + Input.List_MBS[0].ListUE[i].UE_Id + "use channel:" + Input.List_MBS[0].ListUE[i].channel_id);
                }
                for (int i = 0; i < Input.List_SBS.Count; i++)
                {
                    Console.WriteLine("Id S-UE" + Input.List_SBS[i].ListUE[0].UE_Id);
                    for (int j = 0; j < Input.List_SBS[i].ListUE[0].List_channel_id.Count; j++)
                    {
                        Console.WriteLine(Input.List_SBS[i].ListUE[0].List_channel_id[j]);
                    }
                }
                for (int i = 0; i < Input.List_MBS[0].ListChannel.Count; i++)
                {
                    Console.WriteLine("Channel " + (i + 1));
                    for (int j = 0; j < Input.List_MBS[0].ListChannel[i].ListUE.Count; j++)
                    {
                        Console.WriteLine(Input.List_MBS[0].ListChannel[i].ListUE[j].UE_Id);
                    }
           *//*     }*//*
            for(int i = 0; i < Input.List_MBS[0].ListPair.Count; i++)
            {               
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Chose r channel higher linkgain");
                for (int j = 0; j < Input.List_MBS[0].ListPair[i].UE_t.List_channel.Count; j++)
                {
                    Console.WriteLine("Channel" + Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].channel_id + " has linkgain:" + Input.List_MBS[0].ListPair[i].UE_t.List_channel[j].LinkGain);
                }
                Console.WriteLine("*********************************************************");
            }*/
           /* string path1 = Input.path + "\\Input with Power" + Input.D2D_Max_PowerTr + "_" + Input.SUE_Max_PowerTr + ".txt";
            StreamWriter sw1 = new StreamWriter(path1, false);
            sw1.WriteLine("L = 127 + 30*Log(d*10^-3) (dB) (distance: met)");
            sw1.WriteLine("Total interference = (P_tr1/L1) + (P_tr2/L2) + ... + (P_tr10/L10)" +
             "= 10^[(P_tr1(dBm)-L1(dB))/10] + 10^[(P_tr2(dBm)-L2(dB))/10] + ... + 10^[(P_tr10(dBm)-L10(dB))/10]  (W)");
            sw1.WriteLine("SINR(dB) = (P_tr-L)(dBm) - 10log_10_(I)(dB)");
            sw1.WriteLine("-------------------------------------");
            sw1.WriteLine("NumberOfMacroUE: " + Input.NumberofM_UE);
            sw1.WriteLine("NumberOfSBS: " + Input.List_SBS.Count);
            sw1.WriteLine("NumberOfPairD2D: " + Input.List_MBS[0].ListPair.Count);
            sw1.WriteLine("NumberOfChannel: " + Input.NumberofChannel);
            sw1.WriteLine("-------------------------------------");
            sw1.WriteLine("MBSCover: " + Input.MBSCover + " (m)");
          //  sw1.WriteLine("SBSCover: " + Input.SBSCover + " (m)");
            sw1.WriteLine("D2DCover: " + Input.D2DCover + " (m)");
            sw1.WriteLine("MaxPowerofMacro: " + Input.Max_PowerTr + " (dBm)");
            sw1.WriteLine("MaxPowerofDevice: " + Input.D2D_Max_PowerTr + " (dBm)");
            sw1.WriteLine("-------------------------------------");
            sw1.WriteLine("Information of MacroUE: ");
            for (int i = 0; i < Input.List_MBS[0].ListUE.Count; i++)
            {
                sw1.WriteLine("MacroUE has ID: " + Input.List_MBS[0].ListUE[i].UE_Id + " and shadowing=" + Input.List_MBS[0].ListUE[i].Shadowing + " use channel " + Input.List_MBS[0].ListUE[i].channel_id + " has list device using this channel");
                for (int j = 0; j < Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].ListMUE.Count; j++)
                {
                    UE ue = Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].ListMUE[j];
                    sw1.WriteLine("  UE_id:" + ue.UE_Id + " Power= " + ue.PowerTr + " has distance MBS is " +
                                                            ue.Position.distance(ue.Position, Input.List_MBS[0].Position));
                }
                for (int j = 0; j < Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].ListSUE.Count; j++)
                {
                    UE ue = Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].ListSUE[j];
                    sw1.WriteLine("  ID of SUE:" + ue.UE_Id + " Power= " + ue.PowerTr + " has distance MBS is " +
                                                            ue.Position.distance(ue.Position, Input.List_MBS[0].Position));
                }
                for (int j = 0; j < Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].List_device.Count; j++)
                {
                    UE ue = Input.List_MBS[0].ListChannel[Input.List_MBS[0].ListUE[i].channel_id - 1].List_device[j];
                    sw1.WriteLine("  ID of Device:" + ue.UE_Id + " Power= " + ue.PowerTr + " has distance MBS is " +
                                                            ue.Position.distance(ue.Position, Input.List_MBS[0].Position));
                }
                sw1.WriteLine("=>SINR " + Input.List_MBS[0].ListUE[i].SINR);
            }
            for(int i=0; i < Input.List_MBS[0].ListTP.Count; i++)
            {
                sw1.WriteLine("throughput=" + Input.List_MBS[0].ListTP[i]);

            }
            sw1.Close();*/
        }
    }
}
