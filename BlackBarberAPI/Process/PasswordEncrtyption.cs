using BCrypt.Net;
namespace BlackBarberAPI.Process
{
    public class PasswordEncrtyption
    {

        public PasswordEncrtyption()
        {
        }

        public string HashPassword(string password)
        {
            // BCrypt genera automáticamente un 'Salt' y lo incluye en el string resultante
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12);
        }

        // Este método se usa en el LOGIN
        public bool VerifyPassword(string passwordPlano, string passwordHash)
        {
            // Compara el texto plano con el hash guardado en la BD
            return BCrypt.Net.BCrypt.EnhancedVerify(passwordPlano, passwordHash);
        }
    }
}
