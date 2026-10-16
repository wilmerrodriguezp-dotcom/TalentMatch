const API_URL = '/api/v1';

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

