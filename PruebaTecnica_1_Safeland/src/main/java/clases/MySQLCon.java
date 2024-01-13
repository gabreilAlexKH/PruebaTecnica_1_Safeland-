package clases;

import java.sql.*;

public class MySQLCon {

	private static MySQLCon instance = null;
	private Connection con;
	private CallableStatement cs;

	private String db = "jdbc:mysql://186.3.139.231:3306/safelandtest";
	private String user = "testUser";
	private String password = "safeland2023";

	private MySQLCon() throws ClassNotFoundException, SQLException {
		Class.forName("com.mysql.cj.jdbc.Driver");
		startConection();
		setCall();
	}

	public static MySQLCon getInstance() throws ClassNotFoundException, SQLException {

		if (instance == null) {
			instance = new MySQLCon();
		}
		return instance;
	}

	public void startConection() throws ClassNotFoundException, SQLException {
		con = DriverManager.getConnection(db, user, password);
	}

	public void setCall() throws SQLException {
		cs = con.prepareCall("{call sp_getNombreByCodigo(?)}");
	}

	/**
	 * Retorna nombre asignado al código dado como parámetro
	 * 
	 * @param code
	 * @return
	 */
	public String getNombreByCodigo(String code) {

		String nombre;
		try {
			cs.setString(1, code);
			cs.executeQuery();

			ResultSet rs = cs.getResultSet();
			cs.clearParameters();

			rs.next();
			nombre = rs.getString("nombre");

		} catch (SQLException e) {
			nombre = "Gris";
		}

		return nombre;
	}

	public boolean closeConection() {
		try {
			con.close();
			return true;
		} catch (SQLException e) {
			return false;
		}
	}
}
