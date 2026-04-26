using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmpresaUI
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost\\SQLEXPRESS;Database=empresa_db;Trusted_Connection=True;";

        TextBox txtId = new TextBox();
        TextBox txtNombre = new TextBox();
        TextBox txtApellido = new TextBox();
        TextBox txtCargo = new TextBox();

        Button btnCrear = new Button();
        Button btnListar = new Button();
        Button btnActualizar = new Button();
        Button btnEliminar = new Button();

        DataGridView grid = new DataGridView();

        public Form1()
        {
            this.Text = "Sistema Usuarios PRO";
            this.Size = new System.Drawing.Size(900, 550);

            // Inputs
            txtId.Top = 20; txtId.Left = 20; txtId.Width = 60; txtId.PlaceholderText = "ID"; txtId.Enabled = false;
            txtNombre.Top = 20; txtNombre.Left = 90; txtNombre.Width = 150; txtNombre.PlaceholderText = "Nombre";
            txtApellido.Top = 20; txtApellido.Left = 250; txtApellido.Width = 150; txtApellido.PlaceholderText = "Apellido";
            txtCargo.Top = 20; txtCargo.Left = 410; txtCargo.Width = 150; txtCargo.PlaceholderText = "Cargo";

            // Botones
            btnCrear.Text = "Crear";
            btnCrear.Top = 60; btnCrear.Left = 20;
            btnCrear.Click += CrearUsuario;

            btnListar.Text = "Listar";
            btnListar.Top = 60; btnListar.Left = 110;
            btnListar.Click += ListarUsuarios;

            btnActualizar.Text = "Actualizar";
            btnActualizar.Top = 60; btnActualizar.Left = 200;
            btnActualizar.Click += ActualizarUsuario;

            btnEliminar.Text = "Eliminar";
            btnEliminar.Top = 60; btnEliminar.Left = 310;
            btnEliminar.Click += EliminarUsuario;

            // Grid
            grid.Top = 110;
            grid.Left = 20;
            grid.Width = 840;
            grid.Height = 350;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.CellClick += SeleccionarFila;

            // Add controls
            this.Controls.Add(txtId);
            this.Controls.Add(txtNombre);
            this.Controls.Add(txtApellido);
            this.Controls.Add(txtCargo);
            this.Controls.Add(btnCrear);
            this.Controls.Add(btnListar);
            this.Controls.Add(btnActualizar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(grid);
        }

        // CREATE
        private void CrearUsuario(object sender, EventArgs e)
        {
            if (txtNombre.Text == "" || txtApellido.Text == "" || txtCargo.Text == "")
            {
                MessageBox.Show("Completa todos los campos");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO usuarios (nombre, apellido1, cargo, ingreso) VALUES (@n, @a, @c, GETDATE())";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                cmd.Parameters.AddWithValue("@a", txtApellido.Text);
                cmd.Parameters.AddWithValue("@c", txtCargo.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario creado");

                Limpiar();
                ListarUsuarios(null, null);
            }
        }

        // READ
        private void ListarUsuarios(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM usuarios", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                grid.DataSource = dt;
            }
        }

        // UPDATE
        private void ActualizarUsuario(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Selecciona un usuario");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "UPDATE usuarios SET nombre=@n, apellido1=@a, cargo=@c WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", txtNombre.Text);
                cmd.Parameters.AddWithValue("@a", txtApellido.Text);
                cmd.Parameters.AddWithValue("@c", txtCargo.Text);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Actualizado");

                Limpiar();
                ListarUsuarios(null, null);
            }
        }

        // DELETE
        private void EliminarUsuario(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Selecciona un usuario");
                return;
            }

            var confirm = MessageBox.Show("¿Seguro que quieres eliminar?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.No) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM usuarios WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Eliminado");

                Limpiar();
                ListarUsuarios(null, null);
            }
        }

        // SELECT GRID
        private void SeleccionarFila(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = grid.Rows[e.RowIndex];

                txtId.Text = fila.Cells["id"].Value.ToString();
                txtNombre.Text = fila.Cells["nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["apellido1"].Value.ToString();
                txtCargo.Text = fila.Cells["cargo"].Value.ToString();
            }
        }

        // CLEAR
        private void Limpiar()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCargo.Text = "";
        }
    }
}