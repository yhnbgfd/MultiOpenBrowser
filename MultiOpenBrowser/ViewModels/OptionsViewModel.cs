using ReactiveUI;
using System.Reactive;

namespace MultiOpenBrowser.ViewModels
{
    public class OptionsViewModel : ReactiveObject
    {
        public Option Option => GlobalData.Option;
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public OptionsViewModel()
        {
            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
        }

        public async Task SaveAsync()
        {
            await CacheHelper.SetAsync(nameof(MultiOpenBrowser.Core.Entitys.Option), Option);
        }
    }
}
