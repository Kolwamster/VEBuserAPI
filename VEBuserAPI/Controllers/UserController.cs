﻿using Microsoft.AspNetCore.Mvc;

namespace VEBuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IDisposable
    {
        private UserContext _db;
        public UserController()
        {
            _db = new UserContext();
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _db.Users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(Guid id)
        {
            IQueryable<User> users = _db.Users.Where(obj => obj.Id == id);
            return users.First();
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] User value)
        {
            User user = new User();
            user.Id = Guid.NewGuid();
            user.Name = value.Name;
            user.Age = value.Age;
            user.Email = value.Email;
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
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
