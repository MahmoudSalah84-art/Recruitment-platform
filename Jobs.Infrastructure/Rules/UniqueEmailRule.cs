using Jobs.Domain.Entities;
using Jobs.Domain.IRepository.IRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Rules
{
	public class UniqueEmailRule : IUserRepository
	{
		private readonly IUserRepository _userRepository;

		public UniqueEmailRule(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUniqueAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) return false;
			return !await _userRepository.EmailExistsAsync(email);
		}

        public IQueryable<User> Query()
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
