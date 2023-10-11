using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VEBuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IDisposable
    {
        private DataContext _db;

        private void ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentNullException(nameof(user.Name));
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }

            if (user.Age <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(user.Age));
            }
        }

        private void ValidateEmail(User user)
        {
            if (_db.Users.Count(u => u.Email == user.Email) > 0)
            {
                throw new ArgumentException($"{user.Email} is already in use.");
            }
        }

        public UserController()
        {
            _db = new DataContext();
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<object> GetUsers(int pageSize, int pageIndex, string? userName = "", int? age = 0, string? email = "", string? roleName = "")
        {
            var users = _db.Users
                        .Include(user => user.Roles)
                        .Where(u => string.IsNullOrEmpty(userName) || u.Name.Contains(userName))
                        .Where(u => string.IsNullOrEmpty(email) || u.Email.Contains(email))
                        .Where(u => age == 0 || u.Age == age)
                        .Where(u => string.IsNullOrEmpty(roleName) || u.Roles.Any(r => r.Name.Contains(roleName)));
                        
            var page = users.Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize)
                        .ToList();
            return page;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public object GetUser(Guid id)
        {
            var users = _db.Users
                        .Include(user => user.Roles)
                        .Where(u => u.Id == id);
            return users.First();
        }

        // POST api/<UserController>
        [HttpPost]
        public void PostUser([FromBody] User value)
        {
            try
            {
                ValidateUser(value);
                ValidateEmail(value);
                value.Id = Guid.NewGuid();
                _db.Users.Add(value);
                _db.SaveChanges();
            }
            catch (ArgumentNullException)
            {

            }
            catch (ArgumentOutOfRangeException)
            {

            }
            catch (Exception)
            {

            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void PutUser(Guid id, [FromBody] User value)
        {
            try
            {
                ValidateUser(value);
                value.Id = id;
                _db.Users.Update(value);
                _db.SaveChanges();
            }
            catch (ArgumentNullException)
            {

            }
            catch (ArgumentOutOfRangeException)
            {

            }
            catch (Exception)
            {

            }
        }

        // PUT api/<UserController>/AddRole/5
        [HttpPatch("AddRole/{id}")]
        public void AddUserRole(Guid id, [FromBody] Role value) 
        {
            User? user = _db.Users.Find(id);
            Role? role = _db.Roles.Find(value.Id);
            if (user is not null && role is not null)
            {
                user.Roles.Add(role);
                _db.Users.Update(user);
                _db.SaveChanges();
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void DeleteUser(Guid id)
        {
            User user = new User();
            user.Id = id;
            _db.Remove(user);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
