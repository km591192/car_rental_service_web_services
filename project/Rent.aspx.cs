using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Rent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        localhost.WebService dt = new localhost.WebService();
        //MyWS dt = new MyWS();
        DateTime[] dt_arr = dt.GetDateTimes();
        string[] date = dt_arr[1].ToString().Split(' ');
        Label4.Text = date[0];
    }

    WebService1.WebService1 ws1 = new WebService1.WebService1();
    localhost.WebService sendmsg = new localhost.WebService();

    protected void Button2_Click(object sender, EventArgs e)
    {
        String n = TextBox1.Text;
        String s = TextBox2.Text;
        String t = TextBox3.Text;
        String dl = TextBox4.Text;
        if (n == "" || t == "" || dl == "") { Label3.Text = "Please. Enter some information in the field."; return; }

        int cl_id = ws1.returnclientid(TextBox1.Text.Trim().ToString(), TextBox2.Text.Trim().ToString(), TextBox3.Text.Trim().ToString(), TextBox4.Text.Trim().ToString());
        if (cl_id > 0)
        {
            Label3.Text = "You are in our database. Your number is:" + cl_id.ToString();
        }
        else
        {
            ws1.insertclient(n, s, t, dl);

            sendmsg.SendEmail("New client", "New client : " + n + " " + s + ". " + "Tel:" + t + ". " + "Dr. license:" + dl);
        }

        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        Label2.Text = "For show total price choose car id and dates: start  and end";
        var c1 = Calendar5.SelectedDate.Date;
        var c2 = Calendar6.SelectedDate.Date;
        int car_id = Convert.ToInt32(TextBox5.Text.Trim().ToString());
        int cl_id = Convert.ToInt32(TextBox6.Text.Trim().ToString());
        string strstart = c1.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string strend = c2.ToString("yyyy-MM-dd HH:mm:ss.fff");
        int totalprice = Convert.ToInt32(ws1.total_rent_price(c1, c2, car_id).Split('=')[1]);

        int flag = 1;
        flag = ws1.getdateflag(c1, c2, car_id);

        
       if (flag == 1)
        {
            ws1.insertla(strstart, strend,totalprice, cl_id, car_id);

            sendmsg.SendEmail("New lease agreement ", "New lease agreement : Start: " + strstart + " End: " + strend + ". " + "Total price :" + totalprice + ". " + "Client id :" + cl_id.ToString() + ". Car id: " + car_id.ToString());
        }
        if (flag == 0)
        {
            Label2.Text = "CAR IN RENT !!! PLEASE, CHOOSE ANOTHER ONE. OR ERROR IN DATE";
        }
        TextBox5.Text = "";
        TextBox6.Text = "";
    }



    protected void Button3_Click(object sender, EventArgs e)
    {
        String n = TextBox1.Text;
        String s = TextBox2.Text;
        String t = TextBox3.Text;
        String dl = TextBox4.Text;
        if (n == "" || t == "" || dl == "") { Label3.Text = "Please. Enter some information in the field."; return; }

        int cl_id = ws1.returnclientid(TextBox1.Text.Trim().ToString(), TextBox2.Text.Trim().ToString(), TextBox3.Text.Trim().ToString(), TextBox4.Text.Trim().ToString());
        if (cl_id > 0)
        {
            Label3.Text = "You are in our database. Your number is:" + cl_id.ToString();
        }
        else
        {
          Label3.Text = "Please, enter button Add. You are not in our database.";
        }

        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        Label2.Text = "For show total price choose car id and dates: start  and end";
        var c1 = Calendar5.SelectedDate.Date;
        var c2 = Calendar6.SelectedDate.Date;
        int car_id = Convert.ToInt32(TextBox5.Text.Trim().ToString());
        Label2.Text = ws1.total_rent_price(c1, c2, car_id);
        TextBox5.Text = "";

    }



    protected void Button5_Click(object sender, EventArgs e)
    {
        var list = ws1.selectmodel();
        GridView5.DataSource = list;
        GridView5.DataBind();
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        var list = ws1.selectpopcar();
        GridView6.DataSource = list;
        GridView6.DataBind();
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
        var list = ws1.selectavailablecar();
        GridView7.DataSource = list;
        GridView7.DataBind();
    }

    protected void Button8_Click(object sender, EventArgs e)
    {
        var list = ws1.selectpopandavailablecar();
        GridView8.DataSource = list;
        GridView8.DataBind();
    }
}
