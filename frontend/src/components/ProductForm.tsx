type ProductFormProps = {
  nombre: string;
  precio: string;
  stock: string;
  onNombreChange: (value: string) => void;
  onPrecioChange: (value: string) => void;
  onStockChange: (value: string) => void;
  onSubmit: (event: React.FormEvent) => void;
};

export function ProductForm({
  nombre,
  precio,
  stock,
  onNombreChange,
  onPrecioChange,
  onStockChange,
  onSubmit
}: ProductFormProps) {
  return (
    <form onSubmit={onSubmit} className="product-form">
      <label>
        Nombre
        <input value={nombre} onChange={(e) => onNombreChange(e.target.value)} />
      </label>

      <label>
        Precio
        <input type="number" step="0.01" value={precio} onChange={(e) => onPrecioChange(e.target.value)} />
      </label>

      <label>
        Stock
        <input type="number" min="0" value={stock} onChange={(e) => onStockChange(e.target.value)} />
      </label>

      <button type="submit">Crear producto</button>
    </form>
  );
}
