using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public interface IWebElement<T> {
        public T ToElement();
        public void ToWebElement(T element);
    }
}
