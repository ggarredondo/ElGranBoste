namespace SaveUtilities
{
    public interface ISave
    {
        public void Load();
        public void Save();
        public void ApplyChanges();
        public void ChangeGameSlot(int slot);
        public bool IsLoaded();
        public void SetDefault();

        public OptionsSlot Options { get; }

        public GameSlot Game { get; }
    }
}
