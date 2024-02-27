namespace PlorgWeb.WebDTO {
    public interface IWebElementDTO<T> {
        public T FromDTO();
        public void ToDTO(T element);
    }
}
