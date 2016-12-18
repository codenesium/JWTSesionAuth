using Codenesium.DataConversionExtensions;
using Codenesium.Encryption;
using Codenesium.JWTSessionAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JWTTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            SessionManager manager = new SessionManager(
    new EncryptionManager(new Codenesium.Encryption.BCryptor(),
    new Codenesium.Encryption.JWTHelper(),
    new Codenesium.Encryption.HashSHA256(),
    new Codenesium.Encryption.SaltBCrypt()),
    textBoxSecretKey.Text,
    textBoxExpirationInSeconds.Text.ToInt(),
    textBoxSiteName.Text);
            string result = manager.PackagePayload(
                SessionManager.GenerateSessionPayload(
                    DateTime.UtcNow.ToCompleteDateString(),
                    DateTime.UtcNow.AddSeconds(textBoxExpirationInSeconds.Text.ToInt()).ToCompleteDateString(),
                   textBoxEmail.Text,
                   textBoxUserId.Text.ToInt(),
                   textBoxSiteName.Text
                   )
                   );

            textBoxOutput.Text = $@"javascript:document.cookie = ""session={result}""";
        }
    }
}