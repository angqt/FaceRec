using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using KinectSample1.src.DataStructures;


namespace KinectSample1.src.Tools
{
    class DbConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DbConnection()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "FaceDbTraining";
            uid = "root";
            password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }

        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        //Insert statement
        public bool Insert(Person p, List<List<FacePoints>> fp)
        {
            bool retPer = false;
            bool retFace = false;
            bool retFacePts = false;
            bool ret = false;
            string queryPerson = "insert into Person(Name, Surname, PassportNumber) values('"+p.name+"','"+p.surname+"','"+p.passportId+"')";

            if (this.OpenConnection() == true)
            {
                //Create Command

                MySqlCommand cmd = new MySqlCommand(queryPerson, connection);

                try
                {
                    cmd.ExecuteNonQuery();
                    retPer = true;
                }
                catch(Exception e){

                }

                MySqlCommand lstId = new MySqlCommand("select last_insert_id() as lastId from Person", connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = lstId.ExecuteReader();
                int lastId = 0;

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    lastId = Int32.Parse(dataReader["lastId"] + "");
                }

                dataReader.Close();

                MySqlTransaction trc = connection.BeginTransaction();

                string queryFace = "insert into Face(Person_idPerson) values (" + lastId + ")";
                MySqlCommand comm = new MySqlCommand(queryFace, connection);

                comm.Transaction = trc;
                try
                {
                    comm.ExecuteNonQuery();
                    retFace = true;
                }
                catch (Exception e)
                {
                    //trc.Rollback();

                }


                MySqlCommand lstIdFc = new MySqlCommand("select last_insert_id() as lastId from Face", connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReaderFc = lstId.ExecuteReader();
                int lastIdFc = 0;

                //Read the data and store them in the list
                while (dataReaderFc.Read())
                {
                    lastIdFc = Int32.Parse(dataReaderFc["lastId"] + "");
                }

                dataReaderFc.Close();



                for (int i = 0; i < fp.Count; i++)
                {
                    for(int j=0; j<fp[i].Count;j++)
                    {
                        string queryFacePts = "insert into FacePoints(ID,Xcoordinate,Ycoordinate,Zcoordinate,faceId,personId) values (" + j + "," + fp[i][j].x + "," + fp[i][j].y + "," + fp[i][j].z + "," + lastIdFc + "," + lastId + ")";
                        
                        MySqlCommand commPts = new MySqlCommand(queryFacePts, connection);

                        commPts.Transaction = trc;
                        try
                        {
                            commPts.ExecuteNonQuery();
                            retFacePts = true;
                        }
                        catch (Exception e)
                        {
                            //trc.Rollback();
                            Console.WriteLine(e.Message);

                        }
                    }
                }

                trc.Commit();

                ret = retFace && retPer && retFacePts;  

                this.CloseConnection();
            }

            return ret;
        }

        public bool InsertFaces(Person p, List<List<FacePoints>> fp)
        {
            
            bool ret = false;
            bool retFace = false;
            bool retFacePts = false;
            
                MySqlTransaction trc = connection.BeginTransaction();

                string queryFace = "insert into Face(Person_idPerson) values (" + p.id + ")";
                MySqlCommand comm = new MySqlCommand(queryFace, connection);

                comm.Transaction = trc;
                try
                {
                    comm.ExecuteNonQuery();
                    retFace = true;
                }
                catch (Exception e)
                {
                    //trc.Rollback();

                }


                MySqlCommand lstIdFc = new MySqlCommand("select last_insert_id() as lastId from Face", connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReaderFc = lstIdFc.ExecuteReader();
                int lastIdFc = 0;

                //Read the data and store them in the list
                while (dataReaderFc.Read())
                {
                    lastIdFc = Int32.Parse(dataReaderFc["lastId"] + "");
                }

                dataReaderFc.Close();



                for (int i = 0; i < fp.Count; i++)
                {
                    for (int j = 0; j < fp[i].Count; j++)
                    {
                        string queryFacePts = "insert into FacePoints(ID,Xcoordinate,Ycoordinate,Zcoordinate,faceId,personId) values (" + j + "," + fp[i][j].x + "," + fp[i][j].y + "," + fp[i][j].z + "," + lastIdFc + "," + p.id + ")";

                        MySqlCommand commPts = new MySqlCommand(queryFacePts, connection);

                        commPts.Transaction = trc;
                        try
                        {
                            commPts.ExecuteNonQuery();
                            retFacePts = true;
                        }
                        catch (Exception e)
                        {
                            //trc.Rollback();
                            Console.WriteLine(e.Message);

                        }
                    }
                }

                trc.Commit();

                ret = retFace && retFacePts;

                this.CloseConnection();

            return ret;
        }

        //Update statement
        public void Update()
        {
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public List<Person> SelectPeople()
        {
            string query = "SELECT * FROM Person";

            //Create a list to store the result
            List<Person> list = new List<Person>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Person currentPerson = new Person();
                    currentPerson.id = Int32.Parse(dataReader["idPerson"] + "");
                    currentPerson.name = dataReader["Name"] + "";
                    currentPerson.surname = dataReader["Surname"] + "";
                    currentPerson.passportId = dataReader["PassportNumber"] + "";



                    list.Add(currentPerson);
                }

                foreach (Person p in list)
                {
                    //Console.ReadKey();
                }


                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }

        }




        public Person SelectPerson(int id)
        {
            string query = "SELECT * FROM Person where idPerson="+id;

            Person p = new Person();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    
                    p.id = Int32.Parse(dataReader["idPerson"] + "");
                    p.name = dataReader["Name"] + "";
                    p.surname = dataReader["Surname"] + "";
                    p.passportId = dataReader["PassportNumber"] + "";

                }

                


                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return p;
            }
            else
            {
                return p;
            }

        }





        //Select statement
        public List<FacePoints> SelectFace(int PersonId)
        {
            string query = "SELECT * FROM FacePoints Where personId="+ PersonId+ "and faceId=1";

            //Create a list to store the result
            List<FacePoints> list = new List<FacePoints>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    FacePoints facep = new FacePoints();
                    facep.id = Int32.Parse(dataReader["ID"]+"");
                    facep.x = float.Parse(dataReader["Xcoordinate"] + "");
                    facep.y = float.Parse(dataReader["Ycoordinate"] + "");
                    facep.z = float.Parse(dataReader["Zcoordinate"] + "");

                    list.Add(facep);
                }


                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }

        }


        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }


}
