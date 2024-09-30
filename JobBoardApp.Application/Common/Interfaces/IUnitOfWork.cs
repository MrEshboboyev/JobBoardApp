namespace JobBoardApp.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
