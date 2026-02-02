using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class GameResultRepository : GenericRepository<GameResult>, IGameResultRepository
    {
        private readonly StemotionContext _context;
        public GameResultRepository(StemotionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetAttemptCountAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .CountAsync(x => x.StudentId == studentId && x.GameId == gameId);
        }

        public async Task<GameResult?> GetBestScoreAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .Where(x => x.StudentId == studentId && x.GameId == gameId)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.CorrectAnswers)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GameResult>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.GameResults
                .Include(x => x.Game)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.PlayedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameResult>> GetByGameAndStudentAsync(Guid studentId, Guid gameId)
        {
            return await _context.GameResults
                .Where(x => x.StudentId == studentId && x.GameId == gameId)
                .OrderByDescending(x => x.PlayedAt)
                .ToListAsync();
        }
    }
}
