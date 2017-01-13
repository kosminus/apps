using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace WpfApplication18
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection con;
 
        public MainWindow()
        {
            InitializeComponent();
            
            //for creating the sql data file
            // SQLiteConnection.CreateFile("data.sqlite"); 
        
            // open the connection everytime the app is open
            con = new SQLiteConnection("Data Source=data.sqlite;Version=3;");
            con.Open();

            dataGrig.IsReadOnly = true;
            btnAdd.Content = "Add";
            
            //CreateTable();

            BindData();

           }

        //create table
        public void CreateTable()
        {
            string sql = "create table agenda (id INTEGER PRIMARY KEY , nume varchar(30),telefon int, email varchar(30));";
            SQLiteCommand command = new SQLiteCommand(sql, con);
            command.ExecuteNonQuery();
        }
       



        public void BindData()
        {
            string sql = "select * from agenda;";
            SQLiteCommand command = new SQLiteCommand(sql, con);

            SQLiteDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(reader);
            dataGrig.ItemsSource = dt.DefaultView;
        }

        //add
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content=="Add")
            {
            string sql = "insert into agenda(nume, telefon, email) values (@nume, @telefon, @email);";
        SQLiteCommand command = new SQLiteCommand(sql,con);

       
        command.Parameters.AddWithValue("@nume", txtNume.Text);
        command.Parameters.AddWithValue("@telefon", txtTelefon.Text);
        command.Parameters.AddWithValue("@email", txtEmail.Text);


            command.ExecuteNonQuery();
            
            //refresh
            BindData();
            ClearAll();
            }
            else if (btnAdd.Content=="Update")
            {
                string sql = "update agenda set nume=@nume, telefon=@telefon,email= @email where id=@id;";
                SQLiteCommand command = new SQLiteCommand(sql, con);


                command.Parameters.AddWithValue("@nume", txtNume.Text);
                command.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                command.Parameters.AddWithValue("@email", txtEmail.Text);

                //find id of current row
                DataRowView row = (DataRowView)dataGrig.SelectedItems[0];
                int id = Convert.ToInt16(  row["id"]); 
                
                command.Parameters.AddWithValue("@id", id );


                command.ExecuteNonQuery();
                //refresh
                btnAdd.Content = "Add";
                BindData();
                ClearAll();
            }

        }
        //delete
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGrig.SelectedItems.Count > 0 )
            {
                DataRowView row = (DataRowView)dataGrig.SelectedItems[0];
                string sql = "delete from agenda where id="+ row["id"]+";";

                SQLiteCommand command= new SQLiteCommand(sql,con);
                command.ExecuteNonQuery();
                txtBlock.Text = "Succesfully";
                BindData();
            }
            else
            {
                txtBlock.Text= "No rows selected";
            }
        }
        //edit
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
             if (dataGrig.SelectedItems.Count > 0 )
            {
            DataRowView row = (DataRowView)dataGrig.SelectedItems[0];
            
                txtNume.Text=row["nume"].ToString();
                txtTelefon.Text=row["telefon"].ToString();
                txtEmail.Text = row["email"].ToString();
                btnAdd.Content = "Update";
            }
             else
             {
                 txtBlock.Text = "No rows selected";
             }
        }

        public void ClearAll()
        {
            txtNume.Text = "";
            txtTelefon.Text = "";
            txtEmail.Text = "";
            txtBlock.Text = "";

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ClearAll();
            btnAdd.Content = "Add";
        }

    }
}
