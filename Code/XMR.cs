using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Saved.Code.Common;
using static Saved.Code.PoolCommon;

namespace Saved.Code
{
    public class XMRPool
    {
        private bool SendXMRPacketToMiner(Socket oClient, byte[] oData, int iSize, string socketid)
        {
            try
            {
                oClient.Send(oData, iSize, SocketFlags.None);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("was aborted"))
                {

                }
                else
                {
                    bool fPrint = !(ex.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time") || ex.Message.Contains("An existing connection was forcibly closed"));
                    if (fPrint)
                        Log("SEND " + ex.Message);
                }
                return false;
            }
        }
        public bool SubmitBiblePayShare(string socketid)
        {
            try
            {
                XMRJob x = RetrieveXMRJob(socketid);
                if (x.hash == null || x.hash == "")
                {
                    Log("SubmitBBPShare::emptyhash", true);
                    return false;
                }
                byte[] oRX = PoolCommon.StringToByteArr(x.hash);
                double nSolutionDiff = PoolCommon.FullTest(oRX);
                bool fUnique = PoolCommon.IsUnique(x.hash);
                string revBBPHash = PoolCommon.ReverseHexString(x.hash);
                // Check to see if this share actually solved the block:
                NBitcoin.uint256 uBBP = new NBitcoin.uint256(revBBPHash);
                NBitcoin.uint256 uTarget;
                try
                {
                    uTarget = new NBitcoin.uint256(_pool._template.target);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                    uTarget = new NBitcoin.uint256(_pool._template.target);
                }
                
                NBitcoin.arith256 aBBP = new NBitcoin.arith256(uBBP);
                NBitcoin.arith256 aTarget = new NBitcoin.arith256(uTarget);
                int nTest = aBBP.CompareTo(aTarget);

                if (aBBP.CompareTo(aTarget) == -1)
                {
                    /*
                    if (false)
                    {
                        Log("Submit::Submitting JOBID " + x.socketid + " with jobidsubmit nonce " +
                            x.nonce + " at height " + _pool._template.height.ToString() 
                            + " seed " + x.seed + " with target " + _pool._template.target + " and solution " + x.solution, true);
                    }
                    */
                    // We solved the block
                    string poolAddress = GetBMSConfigurationKeyValue("PoolAddress");
                    string hex = PoolCommon.GetBlockForStratumHex(poolAddress, x.seed, x.solution);
                    bool fSuccess = PoolCommon.SubmitBlock(hex);
                    if (fSuccess)
                    {
                        PoolCommon.nLastBoarded = 0;
                        PoolCommon.Leaderboard();

                        string sql9 = "exec insSharev2 @height";
                        SqlCommand command5 = new SqlCommand(sql9);
                        command5.Parameters.AddWithValue("@height", _pool._template.height);

                        try
                        {
                            gData.ExecCmd(command5);
                        }
                        catch (Exception ex2)
                        {
                            Log("insSharev2:" + ex2.Message);
                        }

                        //string sql = "Update Share Set Solved=1 where height=@height";
                        //SqlCommand command = new SqlCommand(sql);
                        //command.Parameters.AddWithValue("@height", _pool._template.height);
                        //gData.ExecCmd(command, false, false, false);
                        Log("SUBMIT_SUCCESS: Success for nonce " + x.nonce + " at height " 
                            + _pool._template.height.ToString() + " hex " + hex);

                        // Save block detail
                        try
                        {
                            int pos;
                            string workerAddress;
                            WorkerInfo worker = GetWorker(socketid);
                            try
                            {
                                pos = worker.moneroaddress.IndexOf(".");
                            }
                            catch (Exception e)
                            {
                                pos = -1;
                            }
                            try
                            {
                                if (worker.bbpaddress != null)
                                    workerAddress = worker.bbpaddress;
                                else
                                    workerAddress = "Unknown";
                            }
                            catch (Exception e)
                            {
                                workerAddress = "Unknown";
                            }
                            string workerName = "";
                            if (pos >= 0)
                                workerName = worker.moneroaddress.Substring(pos + 1);
                            string sqlBlockDetail = "INSERT INTO Blocks (height, bbpaddress, worker, ShareRatio, timestamp) VALUES (@height, @bbpaddress, @worker, @ShareRatio, @timestamp)";
                            SqlCommand commandBlockDetail = new SqlCommand(sqlBlockDetail);
                            commandBlockDetail.Parameters.AddWithValue("@height", _pool._template.height);
                            commandBlockDetail.Parameters.AddWithValue("@bbpaddress", workerAddress);
                            commandBlockDetail.Parameters.AddWithValue("@worker", workerName);
                            commandBlockDetail.Parameters.AddWithValue("@ShareRatio", PoolCommon.roundLuck);
                            PoolCommon.roundLuck = 0;
                            DateTime timestamp = DateTime.Now;
                            commandBlockDetail.Parameters.AddWithValue("@timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            
                            gData.ExecCmd(commandBlockDetail);
                        }  
                        catch (Exception e2)
                        {
                            Log("Block Detail: " + e2.StackTrace + " " + e2.StackTrace);
                        }
                    }
                    else
                    {
                        Log("SUBMITBLOCK: Tried to submit the block for nonce " + x.nonce + " and target " 
                            + _pool._template.target + " with seed " + x.seed + " and solution " 
                            + x.solution + " with hex " + hex + " and failed");
                    }
                   
                    _pool._template.updated = 0; // Forces us to get a new block
                    PoolCommon.GetBlockForStratum();
                }
                else
                {
                    PoolCommon.GetBlockForStratum();
                    if (false)
                    {
                        /*
                        Log("Submit::Submitting::HIGH_HASH JOBID " + x.socketid + " with jobidsubmit nonce " +
                            x.nonce + " at height " + _pool._template.height.ToString()
                                + " seed " + x.seed + " with target " + _pool._template.target + " and solution " + x.solution, false);
                                */
                    }

                }

                try
                {
                    if (dictJobs.Count > 25000)
                    {
                        dictJobs.Clear();
                    }
                }
                catch (Exception ex1)
                {
                    Log("cant find the job " + x.socketid.ToString() + ex1.Message);
                }

                return true;
            }
            catch(Exception ex)
            {
                Log("submitshare::Unable to submit bbp share:  " + ex.Message + ex.StackTrace);
            }
            return false;
        }
            
        private void PersistWorker(WorkerInfo w)
        {
            try
            {
                if (w.IP == null || w.IP == "")
                    return;
                string sql = " IF NOT EXISTS (SELECT moneroaddress FROM worker WHERE moneroaddress='"
                          + w.moneroaddress + "') BEGIN \r\n INSERT INTO Worker (id,added,moneroaddress,bbpaddress,ip) values (newid(),getdate(),'"
                          + Saved.Code.BMS.PurifySQL(w.moneroaddress, 255) + "','" + Saved.Code.BMS.PurifySQL(w.bbpaddress, 255) + "','" + GetIPOnly(w.IP) + "') END";
                gData.Exec(sql);
            }
            catch(Exception ex)
            {
                Log("Exception PW " + ex.Message);
            }
        }

        public static double Add(ref double location1, double value)
        {
            // Borrowed from https://stackoverflow.com/questions/1400465/why-is-there-no-overload-of-interlocked-add-that-accepts-doubles-as-parameters

            double newCurrentValue = location1; // non-volatile read, so may be stale
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }

        private void minerXMRThread(Socket client, TcpClient t, string socketid)
        {
            bool fCharity = false;
            string bbpaddress = "";
            string moneroaddress = "";
            double nTrace = 0;
            string sData = "";
            string sParseData = "";

            try
            {
                client.ReceiveTimeout = 3000;
                client.SendTimeout = 5000;

                while (true)
                {

                    try
                    {
                        if (client.Available > 0)
                        {
                            int size = 0;
                            byte[] data = new byte[256000];
                            size = client.Receive(data);

                            if (size > 0)
                            {
                                // Miner->XMR Pool
                                Stream stmOut = t.GetStream();
                                stmOut.Write(data, 0, size);

                                sData = Encoding.UTF8.GetString(data, 0, data.Length);
                                sData = sData.Replace("\0", "");

                                if (sData.Contains("jsonrpc") && sData.Contains("submit") && sData.Contains("params") && sData.Length < 128)
                                {
                                    if (sData.Contains("{") && sData.Contains("id") && !sData.Contains("}"))
                                    {
                                        Log("XMRPool::Received " + socketid + " truncated message.  ", true);
                                        PoolCommon.iXMRThreadCount--;
                                        client.Close();
                                        PoolCommon.WorkerInfo wban = PoolCommon.Ban(socketid, 1, "BAD-CONFIG");
                                        return;
                                    }
                                }

                                // The Stratum data is first split by \r\n
                                string[] vData = sData.Split("\n");
                                for (int i = 0; i < vData.Length; i++)
                                {
                                    string sJson = vData[i];
                                    if (sJson.Contains("submit"))
                                    {
                                        // See if this is a biblepay share:
                                        if (PoolCommon.fMonero2000)
                                        {
                                            sParseData = sJson;
                                            JObject oStratum = JObject.Parse(sJson);
                                            string nonce = "00000000" + oStratum["params"]["nonce"].ToString();
                                            double nJobID = GetDouble(oStratum["params"]["job_id"].ToString());
                                            string hash = oStratum["params"]["result"].ToString();
                                            XMRJob xmrJob = RetrieveXMRJob(socketid);
                                            string rxheader = xmrJob.blob;
                                            string rxkey = xmrJob.seed;

                                            if (rxheader == null)
                                            {
                                                //Log("cant find the job " + nJobID.ToString());
                                                PoolCommon.WorkerInfo wban = PoolCommon.Ban(socketid, 1, "CANT-FIND-JOB");
                                            }
                                            if (rxheader != null)
                                            {
                                                nonce = nonce.Substring(8, 8);
                                                xmrJob.solution = rxheader.Substring(0, 78) + nonce + rxheader.Substring(86, rxheader.Length - 86);
                                                xmrJob.hash = oStratum["params"]["result"].ToString();
                                                xmrJob.hashreversed = PoolCommon.ReverseHexString(hash);
                                                xmrJob.nonce = nonce;
                                                xmrJob.bbpaddress = bbpaddress;
                                                xmrJob.moneroaddress = moneroaddress;

                                                PutXMRJob(xmrJob);
                                                SubmitBiblePayShare(xmrJob.socketid);
                                                WorkerInfo w2 = PoolCommon.GetWorker(socketid);
                                                w2.receivedtime = UnixTimeStamp();
                                                PoolCommon.SetWorker(w2, socketid);
                                            }
                                        }
                                    }
                                    else if (sJson.Contains("login"))
                                    {
                                        sParseData = sJson;
                                        if (sJson.Contains("User-Agent:") || sJson.Contains("HTTP/1.1"))
                                        {
                                            // Someone is trying to connect to the pool with a web browser?  (Instead of a miner):
                                            Log("XMRPool::Received " + socketid + " Web browser Request ", true);
                                            PoolCommon.iXMRThreadCount--;
                                            client.Close();
                                            PoolCommon.WorkerInfo wban = PoolCommon.Ban(socketid, 1, "BAD-CONFIG: browser agent");
                                            return;
                                        }
                                        JObject oStratum = JObject.Parse(sJson);
                                        dynamic params1 = oStratum["params"];
                                        if (PoolCommon.fMonero2000)
                                        {
                                            moneroaddress = params1["login"].ToString();
                                            bbpaddress = params1["pass"].ToString();
                                            if (bbpaddress.Length != 34 || moneroaddress.Length < 95)
                                            {
                                                PoolCommon.iXMRThreadCount--;
                                                client.Close();
                                                PoolCommon.WorkerInfo wban = PoolCommon.Ban(socketid, 1, "BAD-CONFIG: user="+moneroaddress+" pass="+bbpaddress);
                                                return;
                                            }
                                            WorkerInfo w = PoolCommon.GetWorker(socketid);
                                            w.moneroaddress = moneroaddress;
                                            w.bbpaddress = bbpaddress;
                                            w.IP = GetIPOnly(socketid);
                                            PoolCommon.SetWorker(w, socketid);
                                            PersistWorker(w);
                                        }
                                    }
                                    else if (sJson != "")
                                    {
                                        Console.WriteLine(sJson);
                                    }
                                }


                            } else
                            {
                                // Keepalive (prevents the pool from hanging up on the miner)
                                var json = "{ \"id\": 0, \"method\": \"keepalived\", \"arg\": \"na\" }\r\n";
                                data = Encoding.ASCII.GetBytes(json);
                                Stream stmOut = t.GetStream();
                                stmOut.Write(data, 0, json.Length);
                            }
                        }

                    }
                    catch (ThreadAbortException)
                    {
                        //Log("XMR thread(2) is going down...", true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log("Client: " + ex.Message + ex.StackTrace);
                        if (ex.Message.Contains("An existing connection was forcibly closed"))
                        {
                            Console.WriteLine("ConnectionClosed");
                            return;
                        }
                        else if (ex.Message.Contains("was aborted"))
                        {
                            return;
                        }
                        Console.WriteLine("Error occurred while receiving data " + ex.Message);
                    }


                    // In from XMR Pool -> Miner
                    NetworkStream stmIn = t.GetStream();

                    try
                    {
                        t.ReceiveTimeout = 3000;
                        t.SendTimeout = 5000;

                        if (stmIn.DataAvailable)
                        {
                            byte[] bIn = new byte[128000];
                            int bytesIn = 0;
                            bytesIn = stmIn.Read(bIn, 0, 127999);

                            if (bytesIn > 0)
                            {
                                sData = Encoding.UTF8.GetString(bIn, 0, bytesIn);
                                sData = sData.Replace("\0", "");
                                string[] vData = sData.Split("\n");
                                for (int i = 0; i < vData.Length; i++)
                                {
                                    string sJson = vData[i];
                                    if (sJson.Contains("result"))
                                    {
                                        WorkerInfo w = PoolCommon.GetWorker(socketid);
                                        PoolCommon.SetWorker(w, socketid);
                                        JObject oStratum = JObject.Parse(sJson);
                                        string status = oStratum["result"]["status"].ToString();
                                        int id = (int)GetDouble(oStratum["id"]);
                                        if (id == 1 && status == "OK" && sJson.Contains("blob"))
                                        {
                                            // BiblePay Pool to Miner
                                            double nJobId = GetDouble(oStratum["result"]["job"]["job_id"].ToString());
                                            XMRJob x = RetrieveXMRJob(socketid);
                                            x.blob = oStratum["result"]["job"]["blob"].ToString();
                                            x.target = oStratum["result"]["job"]["target"].ToString();
                                            x.seed = oStratum["result"]["job"]["seed_hash"].ToString();
                                            /*
                                            if (false)
                                                Log("blob " + sJson, true);
                                                */
                                            PutXMRJob(x);
                                        }
                                        else if (id > 1 && status == "OK")
                                        {
                                            // They solved an XMR
                                            int iCharity = fCharity ? 1 : 0;
                                            XMRJob x = RetrieveXMRJob(socketid);
                                            string jobTarget = PoolCommon.ReverseHexString(x.target);
                                            UInt64 iTarget = UInt64.Parse(jobTarget, System.Globalization.NumberStyles.HexNumber);
                                            UInt64 iBase = UInt64.Parse("100000001", System.Globalization.NumberStyles.HexNumber);
                                            double weightedShares = iBase / iTarget;
                                            PoolCommon.InsSharev2(bbpaddress, weightedShares);
                                            Add(ref PoolCommon.roundLuck, weightedShares / _pool._template.expectedShares);
                                        }
                                    }
                                    else if (sJson.Contains("submit"))
                                    {
                                        // Noop
                                    }
                                    else if (sJson.Contains("\"method\":\"job\""))
                                    {
                                        JObject oStratum = JObject.Parse(sJson);
                                        double nJobId = GetDouble(oStratum["params"]["job_id"].ToString());
                                        XMRJob x = RetrieveXMRJob(socketid);
                                        x.blob = oStratum["params"]["blob"].ToString();
                                        x.target = oStratum["params"]["target"].ToString();
                                        x.seed = oStratum["params"]["seed_hash"].ToString();

                                        PutXMRJob(x);
                                    }
                                    else if (sJson != "")
                                    {
                                        Console.WriteLine(sJson);
                                    }
                                }
                                SendXMRPacketToMiner(client, bIn, bytesIn, socketid);
                            }
                        }
                        
                        
                    }
                    catch (Exception ex)
                    {
                        Log("Upstream: " + ex.Message + ex.StackTrace);
                        if (ex.Message.Contains("being aborted"))
                        {
                            //PoolCommon.CloseSocket(client);
                            //return;
                        }
                        else if (!ex.Message.Contains("did not properly respond"))
                        {
                            Log("minerXMRThread[0]: Trace=" + nTrace.ToString() + ":" + ex.Message + " : " + ex.StackTrace);
                            Ban(socketid, 1, ex.Message.Substring(0, 12));

                        }
                    }
                    
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {
                //Log("minerXMRThread is going down...", true);
                return;
            }
            catch (Exception ex)
            {
                Log("Miner: " + ex.Message + ex.StackTrace);
                if (ex.Message.Contains("was aborted"))
                {
                    // Noop
                }
                else if (ex.Message.Contains("forcibly closed"))
                {

                }
                else if (!ex.Message.Contains("being aborted"))
                {
                    Log("minerXMRThread2 v2.0: " + ex.Message + ex.StackTrace + " [sdata=" + sData + "], PARSEDATA     \r\n" + sParseData);

                }
            }

            PoolCommon.iXMRThreadCount = iXMRThreadCount - 1;

        }

        void InitializeXMR()
        {

            retry:
            TcpListener listener = null;
            try
            {
                {
                    IPAddress ipAddress = IPAddress.Parse(GetBMSConfigurationKeyValue("bindip"));
                    listener = new TcpListener(IPAddress.Any, (int)GetDouble(GetBMSConfigurationKeyValue("XMRPort")));
                    listener.Start();
                }
            }
            catch (Exception ex1)
            {
                Log("Problem starting XMR pool:" + ex1.Message);
            }

            while (true)
            {
                //  Complimentary outbound socket
                try
                {
                    Thread.Sleep(10);
                    if (listener.Pending())
                    {
                        Socket client = listener.AcceptSocket();
                        client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
                        client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        
                        string socketid = client.RemoteEndPoint.ToString();
                        PoolCommon.WorkerInfo wban = PoolCommon.Ban(socketid, .25, "XMR-Connect");
                        if (!wban.banned)
                        {
                            PoolCommon.iXMRThreadID++;
                            TcpClient tcp = new TcpClient();
                            tcp.Connect(GetBMSConfigurationKeyValue("XMRExternalPool"), (int)GetDouble(GetBMSConfigurationKeyValue("XMRExternalPort")));
                            ThreadStart starter = delegate { minerXMRThread(client, tcp, socketid); };
                            var childSocketThread = new Thread(starter);
                            PoolCommon.iXMRThreadCount++;
                            childSocketThread.Start();
                        }
                        else
                        {
                            // They are already banned
                            LogBan(socketid + " / " + wban.bbpaddress + " / Connection Refused");
                            PoolCommon.CloseSocket(client);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    Log("XMR Pool is going down...");
                    return;
                }
                catch (Exception ex)
                {
                    Log("InitializeXMRPool v1.3: " + ex.Message);
                    Thread.Sleep(5000);
                    goto retry;
                }
            }
        }

        public XMRPool()
        {
            var t = new Thread(InitializeXMR);
            t.Start();
        }
    }
}

