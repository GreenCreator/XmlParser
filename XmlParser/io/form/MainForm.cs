using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Xml.io.controller;

namespace Xml.io.form
{
    public class MainForm : Form
    {
        private readonly DataAccess _dataAccess;
        private readonly XmlParser _xmlParser;
        private readonly HtmlRenderer _htmlRenderer;

        private WebBrowser webBrowser;
        private DataGridView dataGridView;
        private Button LoadButton;
        private Button DisplayButton;

        public MainForm()
        {
            string projectRootPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(projectRootPath, "database.db");
            string xsltPath = Path.Combine(projectRootPath, "template.xslt");

            _dataAccess = new DataAccess(databasePath);
            _xmlParser = new XmlParser();
            _htmlRenderer = new HtmlRenderer(xsltPath);

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var buttonGrid = new TableLayoutPanel
            {
                RowCount = 1,
                ColumnCount = 2,
                Dock = DockStyle.Top,
                AutoSize = true
            };

            LoadButton = new Button { Text = "Load XML" };
            DisplayButton = new Button { Text = "Display Data" };

            buttonGrid.Controls.Add(LoadButton, 0, 0);
            buttonGrid.Controls.Add(DisplayButton, 1, 0);

            LoadButton.Click += LoadButton_Click;
            DisplayButton.Click += DisplayButton_Click;

            dataGridView = new DataGridView { Dock = DockStyle.Top, Height = 0 };
            webBrowser = new WebBrowser { Dock = DockStyle.Fill };

            Controls.Add(webBrowser);
            Controls.Add(dataGridView);
            Controls.Add(buttonGrid);

            this.ClientSize = new Size(800, 600);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog { Filter = "XML files (*.xml)|*.xml" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fileContent = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);
                    _dataAccess.SaveFileToDatabase(fileName, fileContent);
                    MessageBox.Show("Файл загружен и сохранен в базу данных.");
                }
            }
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            foreach (var xmlContent in _dataAccess.GetAllFileContents())
            {
                var attributes = _xmlParser.ExtractAttributes(xmlContent);
                _htmlRenderer.RenderHtml(attributes, webBrowser);
            }
        }
    }
}