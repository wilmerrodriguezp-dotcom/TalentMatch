type Producto = {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
};

type ProductListProps = {
  productos: Producto[];
};

export function ProductList({ productos }: ProductListProps) {
  return (
    <>
      <h3>Productos disponibles</h3>
      <ul className="product-list">
        {productos.map((producto) => (
          <li key={producto.id}>
            <strong>{producto.nombre}</strong> - ${producto.precio} - Stock: {producto.stock}
          </li>
        ))}
      </ul>
    </>
  );
}
