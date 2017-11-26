public interface IButtonAxisEmitter{
	void RegisterButtonAxisReciever(IButtonAxisReciever reciever);
	void UnregisterButtonAxisReciever(IButtonAxisReciever reciever);
}