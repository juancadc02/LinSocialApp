namespace LinkSocial1.Servicios
{
    /// <summary>
    /// Interfaz que declara el método que se van a utilizar para encriptar la contraseña
    /// </summary>
    public interface ServicioEncriptar
    {
        /// <summary>
        /// Interfaz del metodo que recibe un texto y lo devulve encriptado
        /// </summary>
        /// <param name="texto">Recibe un texto</param>
        /// <returns></returns>
        public string Encriptar(string texto);
    }
}
