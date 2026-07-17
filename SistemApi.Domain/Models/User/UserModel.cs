    namespace SistemApi.Domain.Models.User;

    public class UserModel
    {
        public int Id { get; private set;  }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public RoleType Role { get; private set; }
        public bool IsActivate { get; private set; }
        public DateTime CreateAt { get; private set; }
        
        public UserModel(int id, string email, string password, string name, RoleType role, bool isActive, DateTime createAt = default)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("El email no es valido.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("EL hash de contraseña es requerido.", nameof(password));
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("El nombre completo es requerido.", nameof(name));

            Id = id;
            Email = email;
            Password = password;
            Name = name;
            Role = role;
            IsActivate = isActive;
            CreateAt = createAt;
        }

        public void UpdateProfile(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre completo es requerido.", nameof(name));
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("El email no es válido.", nameof(email));

            Name = name;
            Email = email.Trim().ToLowerInvariant();
        }

        public void ChangeRole(RoleType role) => Role = role;

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("El hash de contraseña no es válido", nameof(password));
            
            Password = password;
        }

        public void Deactivate() => IsActivate = false;
        public void Activate() => IsActivate = true;
    }
