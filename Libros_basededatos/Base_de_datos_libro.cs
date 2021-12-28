using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libros_basededatos
{
    public partial class Base_de_datos_libro : Form
    {
        public Base_de_datos_libro()
        {
            InitializeComponent();
        }
        SqlConnection conexion = new SqlConnection(@"server=DESKTOP-GIBA0R2\SQLEXPRESS; database=TI2021_Jairon; Integrated Security=true");

        private void bntIngresar_Click(object sender, EventArgs e)
        {
                conexion.Open();
                string insertar = "INSERT INTO TablaLibros (cod_Libro, nombreLibro, precioLibro, fechaCompra )VALUES(@cod_Libro, @nombreLibro, @precioLibro, @fechaCompra )";
                SqlCommand comando = new SqlCommand(insertar, conexion);
                comando.Parameters.Add(new SqlParameter("@cod_Libro", this.txtCodigo.Text));
                comando.Parameters.Add(new SqlParameter("@nombreLibro", this.txtNombre.Text));
                comando.Parameters.Add(new SqlParameter("@precioLibro", this.txtPrecio.Text));
                comando.Parameters.Add(new SqlParameter("@fechaCompra", datoFecha.Value));
                comando.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Filas Insertadas Correctamente");
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            Base_de_datos_libro_Load(sender, e);
        }

        private void Base_de_datos_libro_Load(object sender, EventArgs e)
        {
            DataTable dt = getpersona();
            this.comboLibro.DataSource = dt;
            this.comboLibro.DisplayMember = "nombreLibro";
            this.comboLibro.ValueMember = "cod_Libro";
        }
        private DataTable getpersona(string codigo = "")
        {
            string sql = "";
            if (codigo == "")
            {
                sql = "select cod_Libro, nombreLibro, precioLibro, fechaCompra ";
                sql += "from TablaLibros order by nombreLibro, cod_Libro";
            }
            else
            {
                sql = "select cod_Libro, nombreLibro, precioLibro, fechaCompra ";
                sql += "from TablaLibros where cod_Libro=@cod_Libro order by nombreLibro";
            }

            SqlCommand comando = new SqlCommand(sql, conexion);
            if (codigo != "")
            {
                comando.Parameters.Add(new SqlParameter("@cod_Libro", codigo));
            }
            SqlDataAdapter ad1 = new SqlDataAdapter(comando);

            //pasar los datos del adaptador a un datatable
            DataTable dt = new DataTable();
            ad1.Fill(dt);
            return dt;
        }
        private void Mostrar(object sender, EventArgs e)
        {
            //hola
            DataTable dt = getpersona(this.comboLibro.SelectedValue.ToString());
            //mostrar la informacion
            foreach (DataRow row in dt.Rows)
            {
                this.textVerCodigo.Text = row["cod_Libro"].ToString();
                this.textVerNombre.Text = row["nombreLibro"].ToString();
                this.textVerPrecio.Text = row["precioLibro"].ToString();
                this.VerDatoFecha.Value = Convert.ToDateTime(row["fechaCompra"].ToString());

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿ESTAS SEGURO QUE DESEAS ELIMINAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string eliminar = "DELETE FROM TablaLibros WHERE cod_Libro = @cod_Libro";
                SqlCommand cmd3 = new SqlCommand(eliminar, conexion);
                cmd3.Parameters.AddWithValue("@cod_Libro", this.textVerCodigo.Text);
                cmd3.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("PERSONA ELIMINADA CON EXITO", "ELIMINO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DataTable dt = getpersona();
                txtCodigo.Clear();
                txtNombre.Clear();
                txtPrecio.Clear();
                Base_de_datos_libro_Load(sender, e);
            }
            else
            {
                MessageBox.Show("NO SE ELIMINO NINGUN DATO", "CANCELACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿ESTAS SEGURO QUE DESEAS ACTUALIZAR?", "CONFIRMAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conexion.Open();
                string actualizar = "UPDATE TablaLibros SET nombreLibro=@nombreLibro, precioLibro=@precioLibro, fechaCompra=@fechaCompra WHERE cod_Libro=@cod_Libro";
                SqlCommand cmd2 = new SqlCommand(actualizar, conexion);
                cmd2.Parameters.AddWithValue("@cod_Libro", this.textVerCodigo.Text);
                cmd2.Parameters.AddWithValue("@precioLibro", this.textVerPrecio.Text);
                cmd2.Parameters.AddWithValue("@nombreLibro", this.textVerNombre.Text);
                cmd2.Parameters.AddWithValue("@fechaCompra", VerDatoFecha.Value);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Los datos han sido actualizados");
                conexion.Close();
                DataTable dt = getpersona();
                txtCodigo.Clear();
                txtNombre.Clear();
                txtPrecio.Clear();
                Base_de_datos_libro_Load(sender, e);
            }
        }
    }
}
