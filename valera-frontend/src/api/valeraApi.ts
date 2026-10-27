import type { Valera } from '../types/valera'

const API_URL = 'https://localhost:7100/api/Valera'

export async function getValeras(): Promise<Valera[]> {
	const response = await fetch(API_URL)

	if (!response.ok) {
		throw new Error('Не удалсь загрузить список Валер')
	}

	const data: Valera[] = await response.json()

	return data
}

export async function getValeraById(id: number): Promise<Valera> {
	const response = await fetch(`${API_URL}/${id}`)

	if (!response.ok) {
		throw new Error('Не удалось загрузить Валеру')
	}
	return response.json()
}

export async function createValera(name: string): Promise<Valera> {
	const response = await fetch(API_URL, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({ name })
	})

	if (!response.ok) {
		throw new Error('Не удалось создать объект Валеры')
	}

	const data: Valera = await response.json()

	return data
}

export async function deleteValeras(id: number): Promise<void> {
	const response = await fetch(`${API_URL}/${id}`, {
		method: 'DELETE'
	})
	if (!response.ok) {
		throw new Error('Не удалсь удалить объект Валеры')
	}
}
