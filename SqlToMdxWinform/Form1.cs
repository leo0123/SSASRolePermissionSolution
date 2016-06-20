using SqlToMdxLib.DAL;
using SqlToMdxLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace SqlToMdxWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //UsersLineContext db = new UsersLineContext();
            //var q = db.UsersLinePs.Count();//
            SqlToMdxLib.SqlToMdx a = new SqlToMdxLib.SqlToMdx();
            a.replaceAll();
        }
    }
}
