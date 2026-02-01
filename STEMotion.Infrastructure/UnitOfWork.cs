using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using STEMotion.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StemotionContext _context;

        public UnitOfWork(StemotionContext context)
        {
            _context = context;
        }



        // IDisposable Implementation
        private bool disposed = false;

        private IUserRepository _userRepository;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        private IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);

        private IGradeRepository _gradeRepository;
        public IGradeRepository GradeRepository => _gradeRepository ??= new GradeRepository(_context);

        private ISubjectRepository _subjectRepository;
        public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(_context);
        private IChapterRepository _chapterRepository;
        public IChapterRepository ChapterRepository => _chapterRepository ??= new ChapterRepository(_context);
        private ILessonRepository _lessonRepository;
        public ILessonRepository LessonRepository => _lessonRepository ??= new LessonRepository(_context);

        public IPaymentRepository _paymentRepository;
        public IPaymentRepository PaymentRepository => _paymentRepository ??= new PaymentRepository(_context);

        public ISubscriptionRepository _subscriptionRepository;
        public ISubscriptionRepository SubscriptionRepository => _subscriptionRepository ??= new SubscriptionRepository(_context);

        public ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        public ISubscriptionPaymentRepository SubscriptionPaymentRepository => _subscriptionPaymentRepository ??= new SubscriptionPaymentRepository(_context);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
