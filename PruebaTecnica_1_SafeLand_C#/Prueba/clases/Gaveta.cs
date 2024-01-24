namespace clases;

public class Gaveta{

    public string Codigo
    {get;}

    public int Cantidad
    {get;}

    public string Nombre
    {get;}

    public string GavetaNombre
    {get; set;}


    public Gaveta(string codigo, int cantidad, string nombre, string gavetanombre) {
		Codigo = codigo;
		Cantidad = cantidad;
		Nombre = nombre;
		GavetaNombre = gavetanombre;
	}

    public static Gaveta formString(string code) {

		string codigo = code.Substring(0, 3);
		int cantidad = int.Parse(code.Substring(3, 2));
		string nombre = code.Substring(5);

		return new Gaveta(codigo, cantidad, nombre, "Gris");

	}


}