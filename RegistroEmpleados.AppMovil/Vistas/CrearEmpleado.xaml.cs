using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class CrearEmpleado : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://prueba4-1b2d2-default-rtdb.firebaseio.com/");
    public List<Cargo> Cargos { get; set; }
    public CrearEmpleado()
	{
        InitializeComponent();
        ListarCargos();
        BindingContext = this;
    }

    private void ListarCargos()
    {
        var cargos = client.Child("Cargos").OnceAsync<Cargo>();
        Cargos = cargos.Result.Select(x => x.Object).ToList();
    }

    private async void guardarBoton_Clicked(object sender, EventArgs e)
    {
        Cargo cargo = cargoPicker.SelectedItem as Cargo;

        var empleado = new Empleado
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            FechaInicio = fechaInicioPicker.Date,
            Sueldo = int.Parse(sueldoEntry.Text),
            Cargo = cargo
        };

        try
        {
            await client.Child("Empleados").PostAsync(empleado);
            await DisplayAlert("�xito", $"El empleado {empleado.PrimerNombre} {empleado.PrimerApellido} fue guardado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}