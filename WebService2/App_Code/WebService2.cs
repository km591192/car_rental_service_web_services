using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.ServiceModel;

/// <summary>
/// Сводное описание для WebService2
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
// [System.Web.Script.Services.ScriptService]
public class WebService2 : System.Web.Services.WebService
{

    public WebService2()
    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }
    

    [WebMethod]
    public string total_rent_price(DateTime c1, DateTime c2, int car_id)
    {     
        int day_count = 0;
        System.TimeSpan days = TimeSpan.Zero;
        if (c1 != null && c2 != null)
        {
            days = c2.Subtract(c1);
        }
        string tmp_days = days.ToString();
        string[] tmp_days_arr = tmp_days.Split('.');
        if (c1 != c2 && Convert.ToInt32(tmp_days_arr[0]) > 0)
            day_count = Convert.ToInt32(tmp_days_arr[0]);
        if (c1 == c2) day_count = 1;
        return "Total price = " + (day_count * getDailyprice(car_id)).ToString(); 

    }

    [WebMethod]
    private int getDailyprice(int car_id)
    {
    string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

    string SelectCommand = "SELECT model.id, model.price_per_day, model.producer_id, model.model FROM producer INNER JOIN model ON producer.name = model.producer_id";

        DataSet ds = new DataSet();
        int price_per_day = 0;
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter select = new SqlDataAdapter(SelectCommand, databaseConnection);
            select.Fill(ds);
            DataView dv = ds.Tables[0].DefaultView;
            price_per_day = (int)dv.Table.Rows[car_id - 1][1];
        }
        return price_per_day;
    }

    [WebMethod]
    public void deleteclient(int id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string deletecommand = "DELETE FROM client WHERE id = " + id;

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(deletecommand, databaseConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }


    }

    [WebMethod]
    public void insertclient(string n, string s, string t, string dl)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string insertcommand = "INSERT INTO client(Name,Surname,Telephone,dl_num) VALUES('" + n + "','" + s + "','" + t + "','" + dl + "')";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(insertcommand, databaseConnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    [DataContract]
    public class Client
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string dl_num { get; set; }

    }

    [WebMethod]
    public List<Client> selectClient()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT * FROM client";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<Client> clients = new List<Client>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var client = new Client
                {
                    id = Convert.ToInt32(reader["id"]),
                    Name = reader["Name"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    Telephone = reader["Telephone"].ToString(),
                    dl_num = reader["dl_num"].ToString()
                };
                clients.Add(client);
            }

        }
        return clients;
    }


    [WebMethod]
    public void deleteproducer(string name)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string deletecommand = "DELETE FROM producer WHERE name = '" + name + "'";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(deletecommand, databaseConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }


    }

    [WebMethod]
    public void insertproducer(string n, string c)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string insertcommand = "INSERT INTO producer(name,country) VALUES('" + n + "','" + c + "')";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(insertcommand, databaseConnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    [DataContract]
    public class Producer
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string country { get; set; }
    }

    [WebMethod]
    public List<Producer> selectproducer()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT * FROM producer";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<Producer> producers = new List<Producer>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var producer = new Producer
                {
                    name = reader["name"].ToString(),
                    country = reader["country"].ToString()
                };
                producers.Add(producer);
            }

        }
        return producers;
    }



    [WebMethod]
    public void deletecar(int id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string deletecommand = "DELETE FROM car WHERE id =" + id;

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(deletecommand, databaseConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }
    }

    [WebMethod]
    public void insertcar(string reg_num, int model_id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string insertcommand = "INSERT INTO car(reg_num,model_id) VALUES('" + reg_num + "'," + model_id + ")";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(insertcommand, databaseConnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    [DataContract]
    public class Car
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string reg_num { get; set; }

        [DataMember]
        public int model_id { get; set; }
    }

    [WebMethod]
    public List<Car> selectcar()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT * FROM car";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<Car> cars = new List<Car>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var car = new Car
                {
                    id = Convert.ToInt32(reader["id"]),
                    reg_num = reader["reg_num"].ToString(),
                    model_id = Convert.ToInt32(reader["model_id"])
                };
                cars.Add(car);
            }

        }
        return cars;
    }

    [DataContract]
    public class Car_more
    {
        [DataMember]
        public int Car_id { get; set; }

        [DataMember]
        public int Model_id { get; set; }

        [DataMember]
        public string producer_id { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string kpp { get; set; }

        [DataMember]
        public string color { get; set; }
    }

    [WebMethod]
    public List<Car_more> selectmorecar()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT car.id AS Car_id, model.id AS Model_id, model.producer_id, model.model, model.kpp, model.color FROM producer INNER JOIN model ON producer.name = model.producer_id INNER JOIN car ON model.id = car.model_id";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<Car_more> cars = new List<Car_more>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var car = new Car_more
                {
                    Car_id = Convert.ToInt32(reader["Car_id"]),
                    Model_id = Convert.ToInt32(reader["Model_id"]),
                    producer_id = reader["producer_id"].ToString(),
                    model = reader["model"].ToString(),
                    kpp = reader["kpp"].ToString(),
                    color = reader["color"].ToString()
                };
                cars.Add(car);
            }

        }
        return cars;
    }




    [WebMethod]
    public void deletemodel(int id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string deletecommand = "DELETE FROM model WHERE id =" + id;

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(deletecommand, databaseConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }
    }

    [WebMethod]
    public void insertmodel(string c, string m, string p, string col, string kpp, string producer)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string insertcommand = "INSERT INTO model(classm,model,price_per_day,color,kpp,producer_id) VALUES('" + c + "','" + m + "'," + p + ",'" + col + "','" + kpp + "','" + producer + "')";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(insertcommand, databaseConnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    [DataContract]
    public class Model
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string classm { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string price_per_day { get; set; }
        
        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string kpp { get; set; }

        [DataMember]
        public string producer_id { get; set; }
    }

    [WebMethod]
    public List<Model> selectmodel()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT * FROM model";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<Model> models = new List<Model>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var model = new Model
                {
                    id = Convert.ToInt32(reader["id"]),
                    classm = reader["classm"].ToString(),
                    model = reader["model"].ToString(),
                    price_per_day = reader["price_per_day"].ToString(),
                    color = reader["color"].ToString(),
                    kpp = reader["kpp"].ToString(),
                    producer_id = reader["producer_id"].ToString()
                };
                models.Add(model);
            }

        }
        return models;
    }



    [DataContract]
    public class TableinTotalPrice
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string price_per_day { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string producer_id { get; set; }
    }

    [WebMethod]
    public List<TableinTotalPrice> selectTableinTotalPrice()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT model.id, model.price_per_day, model.producer_id, model.model FROM producer INNER JOIN model ON producer.name = model.producer_id";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<TableinTotalPrice> TableinTotalPrices = new List<TableinTotalPrice>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var tableinTotalPrice = new TableinTotalPrice
                {
                    id = Convert.ToInt32(reader["id"]),
                    model = reader["model"].ToString(),
                    price_per_day = reader["price_per_day"].ToString(),
                    producer_id = reader["producer_id"].ToString()
                };
                TableinTotalPrices.Add(tableinTotalPrice);
            }

        }
        return TableinTotalPrices;
    }
}

