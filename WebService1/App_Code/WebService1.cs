using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;

/// <summary>
/// Сводное описание для WebService1
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
// [System.Web.Script.Services.ScriptService]
public class WebService1 : System.Web.Services.WebService
{

    public WebService1()
    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string total_rent_price(DateTime c1, DateTime c2, int car_id)
    {
        int day_count = 0;
        System.TimeSpan days = TimeSpan.Zero;
        if ((c1 != null && c2 != null) && c1 < c2 && c1 >= DateTime.Now.Date && c2 >= DateTime.Now.Date)
        {
            days = c2.Subtract(c1);
        }
        string tmp_days = days.ToString();
        string[] tmp_days_arr = tmp_days.Split('.');
        if ((c1 != c2) && c1 < c2 && c1 >= DateTime.Now.Date && c2 >= DateTime.Now.Date)
        {
            if (Convert.ToInt32(tmp_days_arr[0]) > 0)
                day_count = Convert.ToInt32(tmp_days_arr[0]);
        }
        if (c1 == c2) day_count = 1;
        return "Total price = " + (day_count * getDailyprice(car_id)).ToString();

    }

    [WebMethod]
    private int getDailyprice(int car_id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string SelectCommand = "SELECT car.id, model.producer_id, model.classm, model.model, model.color, model.kpp, model.price_per_day FROM car INNER JOIN model ON car.model_id = model.id INNER JOIN producer ON model.producer_id = producer.name";

        DataSet ds = new DataSet();
        int price_per_day = 0;
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter select = new SqlDataAdapter(SelectCommand, databaseConnection);
            select.Fill(ds);
            DataView dv = ds.Tables[0].DefaultView;
            int row_num = 0;
            for (int i = 0; i < (int)dv.Table.Rows.Count; i++)
            {
                if (car_id == (int)dv.Table.Rows[i][0])
                    row_num = i;
            }

            price_per_day = (int)dv.Table.Rows[row_num][6];
        }
        return price_per_day;
    }

    [WebMethod]
    public int getdateflag(DateTime c1, DateTime c2, int car_id)
    {
        int flag = 1;
        string rn_str = "";

        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string SelectCommand = "SELECT * FROM la";

        DataSet ds = new DataSet();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter select = new SqlDataAdapter(SelectCommand, databaseConnection);
            select.Fill(ds);
            DataView dv2 = ds.Tables[0].DefaultView;

            for (int i = 0; i < (int)dv2.Table.Rows.Count; i++)
            {
                if (car_id == (int)dv2.Table.Rows[i][5])
                    rn_str += i.ToString() + " ";
            }
            string[] arr_rn = rn_str.Split(' ');
            for (int i = 0; i < arr_rn.Length; i++)
            {
                if (arr_rn[i] != "")
                    if ((c1 >= (DateTime)dv2.Table.Rows[Convert.ToInt32(arr_rn[i])][1] && c1 <= (DateTime)dv2.Table.Rows[Convert.ToInt32(arr_rn[i])][2]) ||
                            (c2 >= (DateTime)dv2.Table.Rows[Convert.ToInt32(arr_rn[i])][1] && c2 <= (DateTime)dv2.Table.Rows[Convert.ToInt32(arr_rn[i])][2]))
                        flag = 0;
                if ((c1 <= c2 && c1 >= DateTime.Now.Date && c2 >= DateTime.Now.Date) && flag != 0)
                    flag = 1;
                else flag = 0;
            }
        }
        return flag;
    }

    [WebMethod]
    public void insertla(string strstart, string strend, int totalprice, int cl_id, int car_id)
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string insertcommand = "INSERT INTO la(date_start,date_end,total_price,cl_id,car_id) VALUES('" + strstart + "','" + strend + "'," + totalprice + "," + cl_id + "," + car_id + ")";

        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            databaseConnection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(insertcommand, databaseConnection);
            adapter.InsertCommand.ExecuteNonQuery();
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
    private List<Client> selectClient()
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
    public int returnclientid(string n, string sn, string t, string dl)
    {
        int row_num1 = -1;
        List<Client> list = selectClient();

        for (int i = 0; i < list.Count; i++)
        {
            if (n == (string)list[i].Name.ToString().Trim() &&
                sn == (string)list[i].Surname.ToString().Trim() &&
                t == (string)list[i].Telephone.ToString().Trim() &&
                dl == (string)list[i].dl_num.ToString().Trim())
                row_num1 = i;
        }
        int cl_id = -1;
        if (row_num1 > -1)
            cl_id = list[row_num1].id;
        return cl_id;
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
    public class PopularCar
    {
        [DataMember]
        public int Car_id { get; set; }

        [DataMember]
        public int how_much_times { get; set; }
    }

    [WebMethod]
    public List<PopularCar> selectpopcar()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT c.id AS Car_id, COUNT(l.car_id) AS how_much_times FROM car AS c INNER JOIN la AS l ON c.id = l.car_id INNER JOIN model AS m ON c.model_id = m.id WHERE (m.id IN (SELECT TOP (100) PERCENT m1.id FROM model AS m1 INNER JOIN car AS c1 ON m1.id = c1.model_id INNER JOIN la AS l1 ON c1.id = l1.car_id GROUP BY m1.id ORDER BY COUNT(m1.id) DESC)) GROUP BY l.car_id, m.id, c.model_id, c.id ORDER BY how_much_times DESC";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<PopularCar> cars = new List<PopularCar>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var car = new PopularCar
                {
                    Car_id = Convert.ToInt32(reader["Car_id"]),
                    how_much_times = Convert.ToInt32(reader["how_much_times"])
                };
                cars.Add(car);
            }

        }
        return cars;
    }


    [DataContract]
    public class AvailableCar
    {
        [DataMember]
        public int Car_id { get; set; }

        [DataMember]
        public string producer_id { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string classm { get; set; }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string kpp { get; set; }

        [DataMember]
        public string price_per_day { get; set; }
    }

    [WebMethod]
    public List<AvailableCar> selectavailablecar()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT c.id AS Car_id, model.producer_id, model.model, model.classm, model.color, model.kpp, model.price_per_day FROM car AS c INNER JOIN model ON c.model_id = model.id WHERE(c.id NOT IN(SELECT c1.id FROM car AS c1 INNER JOIN la AS l1 ON c1.id = l1.car_id WHERE({ fn NOW() } BETWEEN l1.date_start AND l1.date_end)))";
            
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<AvailableCar> cars = new List<AvailableCar>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var car = new AvailableCar
                {
                    Car_id = Convert.ToInt32(reader["Car_id"]),
                    producer_id = reader["producer_id"].ToString(),
                    model = reader["model"].ToString(),
                    classm = reader["classm"].ToString(),
                    color = reader["color"].ToString(),
                    kpp = reader["kpp"].ToString(),
                    price_per_day = reader["price_per_day"].ToString()
                };
                cars.Add(car);
            }

        }
        return cars;
    }


    [DataContract]
    public class PopandAvailableCar
    {
        [DataMember]
        public int Car_id { get; set; }

        [DataMember]
        public int how_much_times { get; set; }
    }

    [WebMethod]
    public List<PopandAvailableCar> selectpopandavailablecar()
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\onu\ТРПС\project\App_Data\Carrentdb.mdf;Integrated Security=True";

        string selectcommand = "SELECT c.id AS Car_id, COUNT(l.car_id) AS how_much_times FROM car AS c INNER JOIN la AS l ON c.id = l.car_id INNER JOIN model AS m ON c.model_id = m.id WHERE (m.id IN (SELECT TOP (100) PERCENT m1.id FROM model AS m1 INNER JOIN car AS c1 ON m1.id = c1.model_id INNER JOIN la AS l1 ON c1.id = l1.car_id GROUP BY m1.id ORDER BY COUNT(m1.id) DESC)) AND (c.id NOT IN (SELECT c1.id FROM car AS c1 INNER JOIN la AS l1 ON c1.id = l1.car_id WHERE ({ fn NOW() } BETWEEN l1.date_start AND l1.date_end))) GROUP BY l.car_id, m.id, c.model_id, c.id ORDER BY how_much_times DESC";
        SqlCommand SqlCommand;
        SqlDataReader reader;

        List<PopandAvailableCar> cars = new List<PopandAvailableCar>();
        using (SqlConnection databaseConnection = new SqlConnection(constr))
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand = new SqlCommand(selectcommand, databaseConnection);
            adapter.SelectCommand = new SqlCommand(selectcommand, databaseConnection);
            reader = SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var car = new PopandAvailableCar
                {
                    Car_id = Convert.ToInt32(reader["Car_id"]),
                    how_much_times = Convert.ToInt32(reader["how_much_times"])
                };
                cars.Add(car);
            }

        }
        return cars;
    }


}
