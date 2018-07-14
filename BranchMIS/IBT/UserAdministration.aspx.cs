using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace BranchMIS.IBT
{
    public partial class UserAdministration : System.Web.UI.Page
    {
        private int userRoleValue = 0;
        private int userRoleValue_DataCat = 0;

        private string usrName = "";
        //public static int usr_role = 0;

        protected void Page_Load(object sender, EventArgs e)//Page Load Events
        {
            if (Session["IBT_UserName"] == null)
            {
                string usrValid = "SessionExpired";
                Response.Redirect("~/FAS_Home.aspx?usr=" + usrValid, false);
                return;
            }


            CommonCLS.CommonFunctions clsCom = new CommonCLS.CommonFunctions();
            usrName = Session["IBT_UserName"].ToString();


            //Access controls for 'User Role And Assign Users' Function
            if((usrName == "deshapriya.sooriya")||(usrName == "janaka.indrajith"))
            {
                lb_createUserRole.Visible = true;
                lb_createUserRole.Enabled = true;
            }
            else
            {
                lb_createUserRole.Visible = false;
                lb_createUserRole.Enabled = false;
            }





            string pageName = (System.Web.VirtualPathUtility.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath));

            bool x = clsCom.getPermissions(usrName, pageName);

            if (x == true)
            {
                if (!IsPostBack)
                {
                    lb_createUserRole.Enabled = false;

                    get_userRoleData();
                    check_for_IT_User();
                    populateChkBoxList_IBT_Pages();//IBT Pages
                    populateChkBoxList_IBT_Departments();//IBT Departments
                    get_assignedPages();
                    ddl_usrRoles_PageControls_SelectedIndexChanged(null, null);
                    get_IBT_UserDetails();
                    this.SetFocus(btnSetFocusTop1);


                }
            }

            else
            {
                Response.Redirect("~/FAS_Home.aspx?valid=" + x, false);
                Context.ApplicationInstance.CompleteRequest();
            }


        }

        public void get_userRoleData()//Binding User Roles To 'All' Dropdownlist
        {
            OracleConnection conn_get_usrRoleData = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn_get_usrRoleData.Open();

            OracleCommand cmd_get_usrRoleData = conn_get_usrRoleData.CreateCommand();
            cmd_get_usrRoleData.CommandText = "select t.*, t.rowid from fas_ibt_usr_roles t";
            OracleDataAdapter oda_get_usrRoleData = new OracleDataAdapter(cmd_get_usrRoleData);

            DataTable dt_usrRoleData = new DataTable();

            oda_get_usrRoleData.Fill(dt_usrRoleData);


            ddl_userRoles.DataTextField = "ROLE_DESCRIPTION";//In Assign Pages
            ddl_userRoles.DataValueField = "ROLE_ID";

            ddl_userRoles.DataSource = dt_usrRoleData;
            ddl_userRoles.DataBind();


            ddl_userRoles_D_Category.DataTextField = "ROLE_DESCRIPTION";//In Assign DataCategories
            ddl_userRoles_D_Category.DataValueField = "ROLE_ID";

            ddl_userRoles_D_Category.DataSource = dt_usrRoleData;
            ddl_userRoles_D_Category.DataBind();

            ddl_usrRoles_PageControls.DataTextField = "ROLE_DESCRIPTION";//In Assign Form Controls
            ddl_usrRoles_PageControls.DataValueField = "ROLE_ID";

            ddl_usrRoles_PageControls.DataSource = dt_usrRoleData;
            ddl_usrRoles_PageControls.DataBind();

        }

        //-------------------------------------------------------------------------Assign Pages To User Role---------------------------------------------------------------//

        protected void btn_assignPages_Click(object sender, EventArgs e)//Assign IBT Pages To Each User Role (Button Click Event)
        {
            int userRoleValue = ddl_userRoles.SelectedIndex;

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleTransaction ibtPagesTrans;
            conn.Open();

            ibtPagesTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            int usrRoleValue = int.Parse(ddl_userRoles.SelectedValue.ToString());

            if (chkb_IBTPages.SelectedIndex != -1)
            {
                Int16 Counter = 2;

                foreach (ListItem item in chkb_IBTPages.Items)
                {
                    if (item.Selected)
                    {
                        if (Counter == 2)
                        {
                            Counter = 2;
                        }
                        else
                        {
                            //Counter = -1;
                            Counter = Convert.ToInt16(Counter + 1);
                        }

                        string selectedValue = item.Value;

                        OracleCommand cmd_add_usrPermission = conn.CreateCommand();
                        cmd_add_usrPermission.Transaction = ibtPagesTrans;

                        cmd_add_usrPermission.CommandText = "SP_FAS_IBT_USR_PERMISSIONS";
                        cmd_add_usrPermission.CommandType = CommandType.StoredProcedure;

                        cmd_add_usrPermission.Parameters.Add("vRoleID", OracleType.Int32).Value = usrRoleValue;
                        cmd_add_usrPermission.Parameters.Add("vPageID", OracleType.Int32).Value = selectedValue;
                        cmd_add_usrPermission.Parameters.Add("vDelete", OracleType.Int32).Value = Counter;

                        cmd_add_usrPermission.ExecuteNonQuery();

                        Counter = Convert.ToInt16(Counter + 1);
                    }
                }
            }
            else
            {
                OracleCommand cmd_add_usrPermission = conn.CreateCommand();
                cmd_add_usrPermission.Transaction = ibtPagesTrans;

                cmd_add_usrPermission.CommandText = "SP_FAS_IBT_USR_PERMISSIONS";
                cmd_add_usrPermission.CommandType = CommandType.StoredProcedure;

                cmd_add_usrPermission.Parameters.Add("vRoleID", OracleType.Int32).Value = usrRoleValue;
                cmd_add_usrPermission.Parameters.Add("vPageID", OracleType.Int32).Value = 0;
                cmd_add_usrPermission.Parameters.Add("vDelete", OracleType.Int32).Value = 1;

                cmd_add_usrPermission.ExecuteNonQuery();
            }

            ibtPagesTrans.Commit();

            call_error_msg(true);
            lblResult.Text = "Page Assigning Process Completed Successfully...!";

            this.SetFocus(btnSetFocusTop1);
        }

        private void populateChkBoxList_IBT_Pages()//Data Bind To ChaeckBox List IBT System Pages
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            chkb_IBTPages.Items.Clear();

            OracleCommand cmd_populateChkBoxList = conn.CreateCommand();

            cmd_populateChkBoxList.CommandText = "select * from fas_ibt_permissions tt order by tt.id";

            OracleDataReader odr_populateChkBoxList = cmd_populateChkBoxList.ExecuteReader();

            while (odr_populateChkBoxList.Read())
            {
                ListItem item = new ListItem();
                item.Text = odr_populateChkBoxList["description"].ToString();
                item.Value = odr_populateChkBoxList["id"].ToString();
                //item.Selected = Convert.ToBoolean(odr_populateChkBoxList["1"]);
                chkb_IBTPages.Items.Add(item);
            }

            odr_populateChkBoxList.Close();

            //---------------------------Check Box Checked----------------------------//



            userRoleValue = int.Parse(ddl_userRoles.SelectedValue);

            OracleCommand cmd_getPages = conn.CreateCommand();
            cmd_getPages.CommandText = "select t.PER_ID from fas_ibt_user_permissions t where t.role_id = '" + userRoleValue + "'";

            OracleDataReader odr = cmd_getPages.ExecuteReader();

            List<int> list = new List<int>();

            while (odr.Read())
            {
                list.Add(odr.GetInt32(0));
            }


            foreach (object ids in list)
            {
                int x = int.Parse(ids.ToString());

                foreach (ListItem item in chkb_IBTPages.Items)
                {
                    int y = int.Parse(item.Value.ToString());
                    string pageid = y.ToString();
                    if (x == y)
                    {
                        ListItem currentCheckBox = chkb_IBTPages.Items.FindByValue(pageid);
                        if (currentCheckBox != null)
                        {
                            currentCheckBox.Selected = true;
                        }
                    }
                }
            }

            odr.Close();
            odr_populateChkBoxList.Close();
            conn.Close();
        }

        protected void ddl_userRoles_SelectedIndexChanged(object sender, EventArgs e)//User Roles Dropdownlist Selected Index Changed Event
        {
            userRoleValue = int.Parse(ddl_userRoles.SelectedValue.ToString());
            populateChkBoxList_IBT_Pages();
            this.SetFocus(btnSetFocusTop1);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------Assign Data Categories To User Role-----------------------------------------------------//

        protected void btn_Assign_DataCategories_Click(object sender, EventArgs e)//Assign Data Category Button Click Event
        {
            userRoleValue_DataCat = 0;
            userRoleValue_DataCat = int.Parse(ddl_userRoles_D_Category.SelectedValue);

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleTransaction ibtDataCatTrans;
            conn.Open();
            ibtDataCatTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            if (chkb_IBT_Departments.SelectedIndex != -1)
            {
                Int16 Counter = 2;

                foreach (ListItem item in chkb_IBT_Departments.Items)
                {
                    if (item.Selected)
                    {
                        if (Counter == 2)
                        {
                            Counter = 2;
                        }
                        else
                        {
                            //Counter = -1;
                            Counter = Convert.ToInt16(Counter + 1);
                        }

                        string selectedValue = item.Value;

                        OracleCommand cmd_add_usrDataCat = conn.CreateCommand();
                        cmd_add_usrDataCat.Transaction = ibtDataCatTrans;

                        cmd_add_usrDataCat.CommandText = "SP_FAS_IBT_USR_DATACATEGORY";
                        cmd_add_usrDataCat.CommandType = CommandType.StoredProcedure;

                        cmd_add_usrDataCat.Parameters.Add("vRoleID", OracleType.Int32).Value = userRoleValue_DataCat;
                        cmd_add_usrDataCat.Parameters.Add("vDepartmentID", OracleType.Int32).Value = selectedValue;
                        cmd_add_usrDataCat.Parameters.Add("vDelete", OracleType.Int32).Value = Counter;

                        cmd_add_usrDataCat.ExecuteNonQuery();

                        Counter = Convert.ToInt16(Counter + 1);
                    }
                }
            }
            else
            {
                OracleCommand cmd_add_usrDataCat = conn.CreateCommand();
                cmd_add_usrDataCat.Transaction = ibtDataCatTrans;

                cmd_add_usrDataCat.CommandText = "SP_FAS_IBT_USR_DATACATEGORY";
                cmd_add_usrDataCat.CommandType = CommandType.StoredProcedure;

                cmd_add_usrDataCat.Parameters.Add("vRoleID", OracleType.Int32).Value = userRoleValue_DataCat;
                cmd_add_usrDataCat.Parameters.Add("vDepartmentID", OracleType.Int32).Value = 0;
                cmd_add_usrDataCat.Parameters.Add("vDelete", OracleType.Int32).Value = 1;

                cmd_add_usrDataCat.ExecuteNonQuery();
            }


            ibtDataCatTrans.Commit();
            call_error_msg(true);
            lblResult.Text = "Data Category Assigned Successfully!";

            //this.SetFocus(btnSetFocusTop3);

        }

        private void populateChkBoxList_IBT_Departments()//Data Bind To ChaeckBox List Data Categories
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            chkb_IBT_Departments.Items.Clear();

            OracleCommand cmd_populateChkBoxList_IBT_Departments = conn.CreateCommand();

            cmd_populateChkBoxList_IBT_Departments.CommandText = "select d.department_description, d.value from fas_ibt_departments d order by d.value";

            OracleDataReader odr_populateChkBoxList_IBT_Departments = cmd_populateChkBoxList_IBT_Departments.ExecuteReader();

            while (odr_populateChkBoxList_IBT_Departments.Read())
            {
                ListItem item = new ListItem();
                item.Text = odr_populateChkBoxList_IBT_Departments["department_description"].ToString();
                item.Value = odr_populateChkBoxList_IBT_Departments["value"].ToString();
                chkb_IBT_Departments.Items.Add(item);       
            }

            chkb_IBT_Departments.Items.Insert(0, new ListItem("All", "-1"));

            odr_populateChkBoxList_IBT_Departments.Close();

            //---------------------------Check Box Checked----------------------------//



            userRoleValue_DataCat = int.Parse(ddl_userRoles_D_Category.SelectedValue);

            OracleCommand cmd_dataCat = conn.CreateCommand();
            cmd_dataCat.CommandText = "select r.department_id from fas_ibt_rolecat r where r.role_id = '" + userRoleValue_DataCat + "'";

            OracleDataReader odr_dataCat = cmd_dataCat.ExecuteReader();

            List<int> list = new List<int>();

            while (odr_dataCat.Read())
            {
                list.Add(odr_dataCat.GetInt32(0));
            }


            foreach (object ids in list)
            {
                int x = int.Parse(ids.ToString());

                foreach (ListItem item in chkb_IBT_Departments.Items)
                {
                    int y = int.Parse(item.Value.ToString());
                    string dataCatID = y.ToString();
                    if (x == y)
                    {
                        ListItem currentCheckBox = chkb_IBT_Departments.Items.FindByValue(dataCatID);
                        if (currentCheckBox != null)
                        {
                            currentCheckBox.Selected = true;
                        }
                    }
                }
            }

            odr_dataCat.Close();
            odr_populateChkBoxList_IBT_Departments.Close();
            conn.Close();
        }

        protected void ddl_userRoles_D_Category_SelectedIndexChanged(object sender, EventArgs e)//User Roles Dropdownlist Selected Index Changed Event
        {
            userRoleValue_DataCat = int.Parse(ddl_userRoles_D_Category.SelectedValue.ToString());
            populateChkBoxList_IBT_Departments();
            //this.SetFocus(btnSetFocusTop3);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------Assign Form Controls To User Role-------------------------------------------------------//

        private void populateChkBoxList_FormControls()//Data Bind To ChaeckBox List Form Controls
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            chkb_IBT_FormControls.Items.Clear();

            OracleCommand cmd_populateChkBoxList_FormControls = conn.CreateCommand();

            cmd_populateChkBoxList_FormControls.CommandText = "select f.field_name, f.page_id from fas_ibt_form_controls f where f.page_id ='" + ddl_assignedPages.SelectedValue.ToString() + "'";

            OracleDataReader odr_populateChkBoxList_FormControls = cmd_populateChkBoxList_FormControls.ExecuteReader();

            while (odr_populateChkBoxList_FormControls.Read())
            {
                ListItem item = new ListItem();
                item.Text = odr_populateChkBoxList_FormControls["field_name"].ToString();
                item.Value = odr_populateChkBoxList_FormControls["field_name"].ToString();
                //item.Selected = Convert.ToBoolean(odr_populateChkBoxList["1"]);
                chkb_IBT_FormControls.Items.Add(item);
            }

            odr_populateChkBoxList_FormControls.Close();

            //---------------------------Check Box Checked----------------------------//



            userRoleValue = int.Parse(ddl_userRoles.SelectedValue);

            OracleCommand cmd_getPages = conn.CreateCommand();
            cmd_getPages.CommandText = "select t.field_name from fas_ibt_pagecontrols t where t.role_id = '" + ddl_usrRoles_PageControls.SelectedValue.ToString() + "' and t.page_id = '" + ddl_assignedPages.SelectedValue.ToString() + "' and t.active ='1' ";

            OracleDataReader odr = cmd_getPages.ExecuteReader();

            List<string> list = new List<string>();

            while (odr.Read())
            {
                list.Add(odr.GetString(0));
            }


            foreach (object ids in list)
            {
                string x = ids.ToString();

                foreach (ListItem item in chkb_IBT_FormControls.Items)
                {
                    string y = item.Value.ToString();
                    string pageid = y.ToString();
                    if (x == y)
                    {
                        ListItem currentCheckBox = chkb_IBT_FormControls.Items.FindByValue(pageid);
                        if (currentCheckBox != null)
                        {
                            currentCheckBox.Selected = true;
                        }
                    }
                }
            }

            odr.Close();
            odr_populateChkBoxList_FormControls.Close();
            conn.Close();
        }

        protected void ddl_assignedPages_SelectedIndexChanged(object sender, EventArgs e)//IBT System Pages Dropdownlist Selected Index Changed Event
        {
            populateChkBoxList_FormControls();
            this.SetFocus(btnSetFocusTop4);
        }

        protected void ddl_usrRoles_PageControls_SelectedIndexChanged(object sender, EventArgs e)//User Roles Dropdownlist Selected Index Changed Event
        {
            get_assignedPages();

            ddl_assignedPages_SelectedIndexChanged(null, null);
            this.SetFocus(btnSetFocusTop4);
        }

        private void get_assignedPages()//Data Bind To Assigned Pages Dropdownlist 
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd_assignedPages = conn.CreateCommand();
            cmd_assignedPages.CommandText = "select t.role_id, t.per_id, p.description from fas_ibt_user_permissions t inner join " +
                                            "fas_ibt_permissions p on t.per_id = p.id where t.role_id ='" + ddl_usrRoles_PageControls.SelectedValue.ToString() + "' order by t.per_id";

            OracleDataAdapter oda_assignedPages = new OracleDataAdapter(cmd_assignedPages);
            DataTable dt_assignedPages = new DataTable();

            oda_assignedPages.Fill(dt_assignedPages);

            ddl_assignedPages.DataTextField = "description";
            ddl_assignedPages.DataValueField = "per_id";

            ddl_assignedPages.DataSource = dt_assignedPages;
            ddl_assignedPages.DataBind();
        }

        protected void btnAssignFormControls_Click(object sender, EventArgs e)//Assign Page Controls Button Click Event
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            OracleTransaction ibtFormControlsTrans;
            conn.Open();

            ibtFormControlsTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            int roleID = int.Parse(ddl_usrRoles_PageControls.SelectedValue);
            int pageID = -1;

            string fieldName = "";
            int Active = 0;

            if ((ddl_assignedPages.SelectedValue == "") || (ddl_assignedPages.SelectedValue == null))
            {
                return;
            }
            else
            {
                pageID = int.Parse(ddl_assignedPages.SelectedValue);

                if (chkb_IBT_FormControls.SelectedIndex != -1)
                {
                    Int16 Counter = 2;

                    foreach (ListItem item in chkb_IBT_FormControls.Items)
                    {
                        string itemName = item.Text;
                        string selectedValue = "";
                        int ActiveParameter = 0;

                        if (item.Selected)
                        {

                            if (Counter == 2)
                            {
                                Counter = 2;
                            }
                            else
                            {
                                Counter = Convert.ToInt16(Counter + 1);
                            }
                            ActiveParameter = 1;
                        }

                        OracleCommand cmd_add_FormControls = conn.CreateCommand();
                        cmd_add_FormControls.Transaction = ibtFormControlsTrans;

                        cmd_add_FormControls.CommandText = "SP_FAS_IBT_USR_PAGE_CONTROLS";
                        cmd_add_FormControls.CommandType = CommandType.StoredProcedure;

                        cmd_add_FormControls.Parameters.Add("vRoleID", OracleType.Int32).Value = roleID;
                        cmd_add_FormControls.Parameters.Add("vPageID", OracleType.Int32).Value = pageID;
                        cmd_add_FormControls.Parameters.Add("vFieldName", OracleType.VarChar).Value = itemName;
                        cmd_add_FormControls.Parameters.Add("vActive", OracleType.Int32).Value = ActiveParameter;
                        cmd_add_FormControls.Parameters.Add("vDelete", OracleType.Int32).Value = Counter;


                        cmd_add_FormControls.ExecuteNonQuery();

                        Counter = Convert.ToInt16(Counter + 1);
                    }



                    //conn.Close();
                }
                else
                {
                    OracleCommand cmd_add_FormControls = conn.CreateCommand();
                    cmd_add_FormControls.Transaction = ibtFormControlsTrans;

                    cmd_add_FormControls.CommandText = "SP_FAS_IBT_USR_PAGE_CONTROLS";
                    cmd_add_FormControls.CommandType = CommandType.StoredProcedure;

                    cmd_add_FormControls.Parameters.Add("vRoleID", OracleType.Int32).Value = roleID;
                    cmd_add_FormControls.Parameters.Add("vPageID", OracleType.Int32).Value = pageID;
                    cmd_add_FormControls.Parameters.Add("vFieldName", OracleType.VarChar).Value = "";
                    cmd_add_FormControls.Parameters.Add("vActive", OracleType.Int32).Value = 0;
                    cmd_add_FormControls.Parameters.Add("vDelete", OracleType.Int32).Value = 1;


                    cmd_add_FormControls.ExecuteNonQuery();
                }

                ibtFormControlsTrans.Commit();

                call_error_msg(true);
                lblResult.Text = "Form Controls Assigned Successfully!";

                this.SetFocus(btnSetFocusTop4);
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void get_IBT_UserDetails()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd_ibt_users = conn.CreateCommand();
            cmd_ibt_users.CommandText = "select ra.user_name, ur.role_description, ra.role_id, ra.inactive from fas_ibt_role_assigned ra inner join fas_ibt_usr_roles ur " +
                                        "on ra.role_id = ur.role_id";

            OracleDataReader odr_ibt_users = cmd_ibt_users.ExecuteReader();

            DataTable dt_ibtUsers = new DataTable();

            dt_ibtUsers.Load(odr_ibt_users);


            grdIBTUsers.DataSource = dt_ibtUsers;
            grdIBTUsers.DataBind();

        }

        protected void grdIBTUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string usercode = e.Row.Cells[0].Text;
                DropDownList ddl_IBT_Users = (e.Row.FindControl("grd_ddlUserRole") as DropDownList);
                DropDownList ddl_userStatus = (e.Row.FindControl("grd_userStatus") as DropDownList);

                OracleConnection my_conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
                my_conn.Open();

                OracleCommand cmd_get_userRoles = my_conn.CreateCommand();
                cmd_get_userRoles.CommandText = "select ur.role_id, ur.role_description from fas_ibt_usr_roles ur";

                OracleDataAdapter oda = new OracleDataAdapter(cmd_get_userRoles);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                ddl_IBT_Users.DataSource = dt;

                ddl_IBT_Users.DataTextField = "role_description";
                ddl_IBT_Users.DataValueField = "role_id";

                ddl_IBT_Users.DataBind();

                string usrRole = (e.Row.FindControl("grd_lblUserRole") as Label).Text;
                ddl_IBT_Users.Items.FindByText(usrRole).Selected = true;


                //-----------------------------------------------------------------------------------------------------------//

                OracleCommand cmd_get_userStatus = my_conn.CreateCommand();
                cmd_get_userStatus.CommandText = "SELECT R.INACTIVE FROM fas_ibt_role_assigned R WHERE R.USER_NAME = '" + usercode + "'";

                //OracleDataAdapter oda2 = new OracleDataAdapter(cmd_get_userStatus);
                //DataTable dt2 = new DataTable();
                //oda2.Fill(dt2);
                //ddl_userStatus.DataSource = dt2;

                //ddl_userStatus.DataTextField = "INACTIVE";
                //ddl_userStatus.DataValueField = "INACTIVE";

                //ddl_userStatus.DataBind();
                //ddl_userStatus.ClearSelection();
                ddl_userStatus.Items.Insert(0, new ListItem("Active", "0"));
                ddl_userStatus.Items.Insert(1, new ListItem("Inactive", "1"));

                string usrStatus = (e.Row.FindControl("grd_lblUserStatus") as Label).Text;
                ddl_userStatus.Items.FindByValue(usrStatus).Selected = true;

                //oda.Dispose();
                my_conn.Close();
            }


        }

        protected void Update_Click(object sender, System.EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            string user_code = "";
            int user_role = 0;
            int user_state = -1;

            user_code = gvr.Cells[0].Text;
            DropDownList ddl_IBT_Users = (gvr.FindControl("grd_ddlUserRole") as DropDownList);
            DropDownList ddl_userStatus = (gvr.FindControl("grd_userStatus") as DropDownList);

            user_role = int.Parse(ddl_IBT_Users.SelectedValue);
            user_state = int.Parse(ddl_userStatus.SelectedValue);

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd_update_role_assigned = conn.CreateCommand();
            cmd_update_role_assigned.CommandText = "SP_FAS_IBT_ROLE_UPDATE";
            cmd_update_role_assigned.CommandType = CommandType.StoredProcedure;

            cmd_update_role_assigned.Parameters.Add("vUserName", OracleType.VarChar).Value = user_code;
            cmd_update_role_assigned.Parameters.Add("vRole_ID", OracleType.Int32).Value = user_role;
            cmd_update_role_assigned.Parameters.Add("vInactive", OracleType.Int32).Value = user_state;
            cmd_update_role_assigned.Parameters.Add("StatusRet", OracleType.VarChar, 100).Direction = ParameterDirection.Output;

            cmd_update_role_assigned.ExecuteNonQuery();

            string Status = cmd_update_role_assigned.Parameters["StatusRet"].Value.ToString();
            conn.Close();

            if (Status == "Update Fail")
            {
                call_error_msg(false);
                lblResult.Text = "Update Fail!";
            }
            else
            {
                call_error_msg(true);
                lblResult.Text = Status;
            }

            get_IBT_UserDetails();
            this.SetFocus(btnSetFocusTop5);
        }



        public void call_error_msg(bool x)//------------------------------------------------------------------System Messeges----------------------------------------------//
        {
            if (x == true)
            {
                this.page_result.Visible = true;
                this.page_result.Style.Clear();
                page_result.Attributes["class"] = "alert alert-success";

                //this.page_result.Style.Add("background-color", "#32CD32");
                //this.page_result.Style.Add("border-color", "#32CD32");
                lblResult.Visible = true;
            }
            else
            {
                this.page_result.Visible = true;
                this.page_result.Style.Clear();
                page_result.Attributes["class"] = "alert alert-danger";

                //this.page_result.Style.Add("background-color", "#FF0000");
                //this.page_result.Style.Add("border-color", "#FF0000");
                lblResult.Visible = true;
                this.lblResult.Style.Add("color", "#ffff");
            }
        }

        protected void lb_createUserRole_Click(object sender, EventArgs e)//-------------------User Role And Users Assigning Popup-----------------------------------------//
        {
            get_UserRoles();
            mpe.Show();
        }

        protected void btn_CreateRole_Click(object sender, EventArgs e)//----------------------Create User Role------------------------------------------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            try
            {
                int maxId = 0;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select max(t.role_id) from fas_ibt_usr_roles t";

                OracleDataReader odr = cmd.ExecuteReader();
                while (odr.Read())
                {
                    maxId = int.Parse(odr[0].ToString());
                }

                // txtRoleDescription.Text = "";

                if ((txtUserRole.Text != "") && (maxId != 0) && (ddlCompany.SelectedIndex != 0))
                {
                    maxId = maxId + 1;
                    string role = txtUserRole.Text.Trim();
                    string company = ddlCompany.SelectedItem.Text;

                    OracleCommand cmd2 = conn.CreateCommand();
                    cmd2.CommandText = "insert into fas_ibt_usr_roles values (" + maxId + ",'" + role + "','" + company + "')";

                    cmd2.ExecuteNonQuery();
                    call_error_msg(true);

                    lblResult.Text = "New User Role Assigned Successfully...!";
                    get_UserRoles();//Popup
                    get_userRoleData();//Whole Page
                    get_IBT_UserDetails();//GridView Data Bind

                    reset_popupControls();

                    conn.Close();
                }
                else
                {
                    conn.Close();
                    call_error_msg(false);
                    lblResult.Text = "Process Faild, Please Try Again...!";
                    reset_popupControls();
                    return;
                }
            }
            catch (Exception ex)
            {
                call_error_msg(false);
                lblResult.Text = ex.ToString();
            }
            finally
            {
                if (conn.State != null)
                {
                    conn.Close();
                }
            }
        }

        protected void get_UserRoles()//-------------------------------------------------------Get User Roles To Popup Dropdown Lists--------------------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd_get_userRoles = conn.CreateCommand();
            cmd_get_userRoles.CommandText = "select t.role_description, t.role_id from fas_ibt_usr_roles t order by t.role_id";

            OracleDataAdapter oda_getUserRoles = new OracleDataAdapter(cmd_get_userRoles);

            DataTable dt_userRoles = new DataTable();

            oda_getUserRoles.Fill(dt_userRoles);

            ddl_UserRole_popup.DataTextField = "role_description";
            ddl_UserRole_popup.DataValueField = "role_id";

            ddl_UserRole_popup.DataSource = dt_userRoles;
            ddl_UserRole_popup.DataBind();

            ddl_UserRole_popup.Items.Insert(0, new ListItem("--Please Select--", "-1"));

        }

        protected void btnAssignUser_Click(object sender, EventArgs e)//-----------------------Assign New Users To IBT System----------------------------------------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            try
            {
                if ((txt_UserName.Text != "") && (ddl_UserRole_popup.SelectedIndex != 0))
                {

                    string ad_name = txt_UserName.Text.Trim();
                    int roleId = int.Parse(ddl_UserRole_popup.SelectedValue);

                    OracleCommand cmd_assign_user = conn.CreateCommand();
                    cmd_assign_user.CommandText = "insert into fas_ibt_role_assigned values ('" + ad_name + "' , " + roleId + ", 0 )";
                    cmd_assign_user.ExecuteNonQuery();

                    conn.Close();

                    call_error_msg(true);
                    lblResult.Text = "User Assigned Successfully...!";
                    reset_popupControls();
                    get_IBT_UserDetails();
                }
                else
                {
                    call_error_msg(false);
                    lblResult.Text = "User Assign Fail...!";
                    reset_popupControls();
                    get_IBT_UserDetails();
                }
            }
            catch (Exception ex)
            {
                call_error_msg(false);
                lblResult.Text = ex.ToString();
                //throw;
            }
            finally
            {
                if (conn.State != null)
                {
                    conn.Close();
                }
            }

        }

        protected void check_for_IT_User()//---------------------------------------------------Supper User Validation For Create User Roles And Assign Users---------------//
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["ORAWF"].ToString());
            conn.Open();

            OracleCommand cmd_get_users = conn.CreateCommand();
            cmd_get_users.CommandText = "select ttt.user_name from fas_ibt_role_assigned ttt where ttt.user_name in ('deshapriya.sooriya','janaka.indrajith') and ttt.inactive = 0 and ttt.role_id = 1";

            OracleDataReader odr_getUsers = cmd_get_users.ExecuteReader();

            DataTable dt_users = new DataTable();

            dt_users.Load(odr_getUsers);


            List<string> ibtUsers = new List<string>();

            foreach (DataRow row in dt_users.Rows)
            {
                ibtUsers.Add(row["user_name"].ToString());
            }

            Array userNames = ibtUsers.ToArray();

            foreach (string i in ibtUsers)
            {
                if (i == usrName)
                {
                    lb_createUserRole.Enabled = true;
                    return;
                }
                else
                {
                    lb_createUserRole.Enabled = false;
                }
            }
        }

        private void reset_popupControls()//---------------------------------------------------Clear Controls Inside Popup-------------------------------------------------//
        {
            txt_UserName.Text = "";
            txtUserRole.Text = "";
            ddl_UserRole_popup.ClearSelection();
            ddlCompany.ClearSelection();
        }

    }
}