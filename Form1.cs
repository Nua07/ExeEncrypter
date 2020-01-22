using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ExeEncrypter
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();

            FilePathTextbox.AllowDrop = true;
            FilePathTextbox.DragEnter += new DragEventHandler(DragEnter);
            FilePathTextbox.DragDrop += new DragEventHandler(DragDrop);
        }

        void DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }


        void DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 1)
            {
                string file = files[0];

                if (Path.GetExtension(file) != ".exe")
                {
                    MessageBox.Show("확장자가 .exe가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    FilePathTextbox.Text = file;
                }
            }
            else
            {
                MessageBox.Show("파일을 하나만 드래그 해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void FileSelectButton_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string file = openFile.FileName;

                if (Path.GetExtension(file) != ".exe")
                {
                    MessageBox.Show("확장자가 .exe가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    FilePathTextbox.Text = file;
                }
            }
        }

        void BuildButton_Click(object sender, EventArgs e)
        {
            if (FilePathTextbox.Text == "")
            {
                MessageBox.Show("파일이 선택되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (KeyTextbox.Text == "")
            {
                MessageBox.Show("키가 비어있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (KeyTextbox.Text.Contains("#"))
            {
                MessageBox.Show("키에 #이 있습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Compile(FilePathTextbox.Text);
            }
        }

        void Compile(string payloadPath)
        {
            string BuildPath = Path.Combine(Environment.CurrentDirectory, "Build", Path.GetFileNameWithoutExtension(payloadPath));

            if (!Directory.Exists(BuildPath))
            {
                Directory.CreateDirectory(BuildPath);
            }

            string[] referencedAssemblies = new string[] {
                    "System.dll",
                    "System.Windows.Forms.dll",
                    "Microsoft.CSharp.dll",
                    "System.Dynamic.Runtime.dll",
                    "System.Core.dll",
                };
            Dictionary<string, string> providerOptions = new Dictionary<string, string>() {
                    {"CompilerVersion", "v4.0" }
                };

            /*
            var compilerOptions = "/target:library /platform:x86 /optimize+";
            using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(providerOptions))
            {
                CompilerParameters compilerParameters = new CompilerParameters(referencedAssemblies)
                {
                    GenerateExecutable = false,
                    OutputAssembly = Path.Combine(BuildPath, "RunPE" + ".dll"),
                    CompilerOptions = compilerOptions,
                    TreatWarningsAsErrors = false,
                    IncludeDebugInformation = false,
                    TempFiles = new TempFileCollection(BuildPath, false),
                };
                

                CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, Properties.Resources.RunPE);
                if (compilerResults.Errors.Count > 0)
                {
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", compilerError.ErrorText,
                            compilerError.Line, compilerError.Column, compilerError.FileName));
                    }
                }
            }
            */
            MessageBox.Show("Build Started");

            var compilerOptions = "/target:winexe /platform:x86 /optimize+";

            using (CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider(providerOptions))
            {
                CompilerParameters compilerParameters = new CompilerParameters(referencedAssemblies)
                {
                    GenerateExecutable = true,
                    OutputAssembly = Path.Combine(BuildPath, Path.GetFileName(payloadPath)),
                    CompilerOptions = compilerOptions,
                    TreatWarningsAsErrors = false,
                    IncludeDebugInformation = false,
                    TempFiles = new TempFileCollection(BuildPath, false),
                };

                using (ResourceWriter rw = new ResourceWriter(Path.Combine(BuildPath, Path.GetFileNameWithoutExtension(payloadPath) + ".resources")))
                {
                    rw.AddResource("payload", AES_Encrypt(File.ReadAllBytes(FilePathTextbox.Text), KeyTextbox.Text));
                    rw.Generate();
                }

                compilerParameters.EmbeddedResources.Add(Path.Combine(BuildPath, Path.GetFileNameWithoutExtension(payloadPath) + ".resources"));
                var Loader = Properties.Resources.Loader.Replace("#filename", Path.GetFileNameWithoutExtension(payloadPath)).Replace("#password", KeyTextbox.Text);
                //MessageBox.Show(Loader);
                CompilerResults compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, Loader);
                if (compilerResults.Errors.Count > 0)
                {
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", compilerError.ErrorText,
                            compilerError.Line, compilerError.Column, compilerError.FileName));
                    }
                }
                else
                {
                    MessageBox.Show("Build Success", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] encryptedBytes = null;
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

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }
    }
}