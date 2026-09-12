const API_URL = 'https://localhost:7001/api/v1';

export async function login(username: string, password: string) {
  const response = await fetch(`${API_URL}/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ userName: username, password })
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || 'Credenciales inválidas.');
  }

  return response.json();
}

export async function getProductos(token: string) {
  const response = await fetch(`${API_URL}/productos`, {
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    }
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || 'No se pudo cargar la lista de productos.');
  }

  return response.json();
}

export async function createProducto(token: string, producto: { nombre: string; precio: number; stock: number }) {
  const response = await fetch(`${API_URL}/productos`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(producto)
  });

  if (!response.ok) {
    const data = await response.json();
    throw new Error(data.title ?? 'No se pudo crear el producto.');
  }

  return response.json();
}
