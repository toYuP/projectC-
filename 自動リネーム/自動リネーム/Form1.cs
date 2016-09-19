using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自動リネーム
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getFolderPath();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private static string title = "";
        private static int number = 1;
        private static string createFilePash = "";
        private System.IO.FileSystemWatcher watcher = null;
        private void getFolderPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示するもの
            fbd.Description = "フォルダを選択してください";
            fbd.RootFolder = Environment.SpecialFolder.MyPictures;
            fbd.SelectedPath = @"C:\Windows";


            //ユーザーが新規でファイル作成できる
            fbd.ShowNewFolderButton = true;

            //ダイアログ表示
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
            folderMonitor();
        }

        private void folderMonitor()
        {
            if (watcher != null) monitorEnd();
            //初期化
            title = textBox3.Text;
            number = 1;

            watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = textBox1.Text;
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.LastAccess
                | System.IO.NotifyFilters.LastWrite
                | System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.DirectoryName);
            //すべてのファイルを監視
            watcher.Filter = "";
            //UIのスレッドにマーシャリングする
            //コンソールアプリケーションでの使用では必要ない
            watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);

            //監視を開始する
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("監視を開始しました。");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            monitorEnd();
        }

        //監視を終了
        private void monitorEnd()
        {
            //監視を終了
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            watcher = null;
            textBox2.Text = "監視を終了しました。";
        }

        //イベントハンドラ
        private void watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Created:
                    renameFile(e.FullPath);
                    break;
            }
        }

        private void renameFile(string fullFilePath)
        {
            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(fullFilePath);
            if(createFilePash == "")
            {
                createFilePash = new System.IO.FileInfo(fullFilePath).DirectoryName + "\\";
            }

            try
            {
                System.IO.File.Move(fullFilePath, createFilePash + number + "_" + title + System.IO.Path.GetExtension(fullFilePath));
                System.IO.File.Delete(fullFilePath);
            }
            catch(Exception)
            { 
                //特に何もしない
            }
            finally
            {
                number++;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
