using CodexAPI.Data;
using CodexAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CodexAPI.Services;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario> AddAsync(Usuario usuario);
    void Update(Usuario usuario);
    void Delete(Usuario usuario);
    Task SaveAsync();
}

public class UsuarioRepository : IUsuarioRepository
{
    private readonly CodexDbContext _context;

    public UsuarioRepository(CodexDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        var result = await _context.Usuarios.AddAsync(usuario);
        return result.Entity;
    }

    public void Update(Usuario usuario)
    {
        usuario.DataAtualizacao = DateTime.UtcNow;
        _context.Usuarios.Update(usuario);
    }

    public void Delete(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface IAreaRepository
{
    Task<Area?> GetByIdAsync(int id);
    Task<IEnumerable<Area>> GetAllAsync();
    Task<IEnumerable<Area>> GetAllWithDisciplinasAsync();
    Task<Area> AddAsync(Area area);
    void Update(Area area);
    void Delete(Area area);
    Task SaveAsync();
}

public class AreaRepository : IAreaRepository
{
    private readonly CodexDbContext _context;

    public AreaRepository(CodexDbContext context)
    {
        _context = context;
    }

    public async Task<Area?> GetByIdAsync(int id)
    {
        return await _context.Areas.Include(a => a.Disciplinas).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Area>> GetAllAsync()
    {
        return await _context.Areas.Where(a => a.Ativo).OrderBy(a => a.Ordem).ToListAsync();
    }

    public async Task<IEnumerable<Area>> GetAllWithDisciplinasAsync()
    {
        return await _context.Areas
            .Where(a => a.Ativo)
            .Include(a => a.Disciplinas!.Where(d => d.Ativo))
            .OrderBy(a => a.Ordem)
            .ToListAsync();
    }

    public async Task<Area> AddAsync(Area area)
    {
        var result = await _context.Areas.AddAsync(area);
        return result.Entity;
    }

    public void Update(Area area)
    {
        area.DataCriacao = DateTime.UtcNow;
        _context.Areas.Update(area);
    }

    public void Delete(Area area)
    {
        _context.Areas.Remove(area);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface IDisciplinaRepository
{
    Task<Disciplina?> GetByIdAsync(int id);
    Task<IEnumerable<Disciplina>> GetByAreaIdAsync(int areaId);
    Task<IEnumerable<Disciplina>> GetAllAsync();
    Task<Disciplina> AddAsync(Disciplina disciplina);
    void Update(Disciplina disciplina);
    void Delete(Disciplina disciplina);
    Task SaveAsync();
}

public class DisciplinaRepository : IDisciplinaRepository
{
    private readonly CodexDbContext _context;

    public DisciplinaRepository(CodexDbContext context)
    {
        _context = context;
    }

    public async Task<Disciplina?> GetByIdAsync(int id)
    {
        return await _context.Disciplinas
            .Include(d => d.Topicos)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Disciplina>> GetByAreaIdAsync(int areaId)
    {
        return await _context.Disciplinas
            .Where(d => d.AreaId == areaId && d.Ativo)
            .Include(d => d.Topicos)
            .OrderBy(d => d.Ordem)
            .ToListAsync();
    }

    public async Task<IEnumerable<Disciplina>> GetAllAsync()
    {
        return await _context.Disciplinas.Where(d => d.Ativo).OrderBy(d => d.Ordem).ToListAsync();
    }

    public async Task<Disciplina> AddAsync(Disciplina disciplina)
    {
        var result = await _context.Disciplinas.AddAsync(disciplina);
        return result.Entity;
    }

    public void Update(Disciplina disciplina)
    {
        disciplina.DataCriacao = DateTime.UtcNow;
        _context.Disciplinas.Update(disciplina);
    }

    public void Delete(Disciplina disciplina)
    {
        _context.Disciplinas.Remove(disciplina);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface ITopicoRepository
{
    Task<Topico?> GetByIdAsync(int id);
    Task<IEnumerable<Topico>> GetByDisciplinaIdAsync(int disciplinaId);
    Task<IEnumerable<Topico>> GetAllAsync();
    Task<Topico> AddAsync(Topico topico);
    void Update(Topico topico);
    void Delete(Topico topico);
    Task SaveAsync();
}

public class TopicoRepository : ITopicoRepository
{
    private readonly CodexDbContext _context;

    public TopicoRepository(CodexDbContext context)
    {
        _context = context;
    }

    public async Task<Topico?> GetByIdAsync(int id)
    {
        return await _context.Topicos
            .Include(t => t.ProgressosTopicos)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topico>> GetByDisciplinaIdAsync(int disciplinaId)
    {
        return await _context.Topicos
            .Where(t => t.DisciplinaId == disciplinaId && t.Ativo)
            .Include(t => t.ProgressosTopicos)
            .OrderBy(t => t.Ordem)
            .ToListAsync();
    }

    public async Task<IEnumerable<Topico>> GetAllAsync()
    {
        return await _context.Topicos.Where(t => t.Ativo).OrderBy(t => t.Ordem).ToListAsync();
    }

    public async Task<Topico> AddAsync(Topico topico)
    {
        var result = await _context.Topicos.AddAsync(topico);
        return result.Entity;
    }

    public void Update(Topico topico)
    {
        topico.DataCriacao = DateTime.UtcNow;
        _context.Topicos.Update(topico);
    }

    public void Delete(Topico topico)
    {
        _context.Topicos.Remove(topico);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface IProgressoTopicoRepository
{
    Task<ProgressoTopico?> GetByIdAsync(int id);
    Task<ProgressoTopico?> GetByUsuarioAndTopicoAsync(int usuarioId, int topicoId);
    Task<IEnumerable<ProgressoTopico>> GetByUsuarioIdAsync(int usuarioId);
    Task<IEnumerable<ProgressoTopico>> GetByTopicoIdAsync(int topicoId);
    Task<ProgressoTopico> AddAsync(ProgressoTopico progresso);
    void Update(ProgressoTopico progresso);
    void Delete(ProgressoTopico progresso);
    Task SaveAsync();
}

public class ProgressoTopicoRepository : IProgressoTopicoRepository
{
    private readonly CodexDbContext _context;

    public ProgressoTopicoRepository(CodexDbContext context)
    {
        _context = context;
    }

    public async Task<ProgressoTopico?> GetByIdAsync(int id)
    {
        return await _context.ProgressosTopicos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProgressoTopico?> GetByUsuarioAndTopicoAsync(int usuarioId, int topicoId)
    {
        return await _context.ProgressosTopicos
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.TopicoId == topicoId);
    }

    public async Task<IEnumerable<ProgressoTopico>> GetByUsuarioIdAsync(int usuarioId)
    {
        return await _context.ProgressosTopicos
            .Where(p => p.UsuarioId == usuarioId)
            .Include(p => p.Topico)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProgressoTopico>> GetByTopicoIdAsync(int topicoId)
    {
        return await _context.ProgressosTopicos
            .Where(p => p.TopicoId == topicoId)
            .ToListAsync();
    }

    public async Task<ProgressoTopico> AddAsync(ProgressoTopico progresso)
    {
        progresso.DataInicio = DateTime.UtcNow;
        var result = await _context.ProgressosTopicos.AddAsync(progresso);
        return result.Entity;
    }

    public void Update(ProgressoTopico progresso)
    {
        progresso.DataAtualizacao = DateTime.UtcNow;
        if (progresso.Concluido && progresso.DataConclusao == null)
            progresso.DataConclusao = DateTime.UtcNow;
        _context.ProgressosTopicos.Update(progresso);
    }

    public void Delete(ProgressoTopico progresso)
    {
        _context.ProgressosTopicos.Remove(progresso);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
