package clases;

public class Gaveta {

    private String gaveta;
    private int cantidad;
    private String nombre;
    private String gavetanombre;

    public Gaveta(String gaveta, int cantidad, String nombre, String gavetanombre) {
        this.gaveta = gaveta;
        this.cantidad = cantidad;
        this.nombre = nombre;
        this.gavetanombre = gavetanombre;
    }

    public static Gaveta formString(String code) {

        String gaveta = code.substring(0, 3);
        int cantidad = Integer.valueOf(code.substring(3, 5));
        String nombre = code.substring(5);

        return new Gaveta(gaveta, cantidad, nombre, "Gris");

    }

    public String getGaveta() {
        return gaveta;
    }

    public int getCantidad() {
        return cantidad;
    }

    public String getNombre() {
        return nombre;
    }

    public String getGavetanombre() {
        return gavetanombre;
    }

    public void setGavetanombre(String gavetanombre) {
        this.gavetanombre = gavetanombre;
    }

}
