import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getValeraById } from '../api/valeraApi'
import type { Valera } from '../types/valera'

export function ValeraStats() {
	const { id } = useParams()

	const [valera, setValera] = useState<Valera | null>(null)
	const [error, setError] = useState('')
	const [isLoading, setIsLoading] = useState(true)

	useEffect(() => {
		async function loadValera() {
			if (!id || !Number.isInteger(Number(id)) || Number(id) <= 0) {
				setError('Некорректный ID персонажа')
				setIsLoading(false)
				return
			}

			try {
				setError('')
				setIsLoading(true)

				const data = await getValeraById(Number(id))
				setValera(data)
			} catch (error) {
				console.error('Ошибка загрузки Валеры:', error)
				setError('Не удалось загрузить персонажа')
			} finally {
				setIsLoading(false)
			}
		}

		loadValera()
	}, [id])

	if (isLoading) {
		return (
			<main className="min-h-screen bg-zinc-950 p-6 text-white">
				<p>Загружаем Валеру...</p>
			</main>
		)
	}

	if (error || !valera) {
		return (
			<main className="min-h-screen bg-zinc-950 p-6 text-white">
				<p className="text-red-400">{error || 'Персонаж не найден'}</p>

				<Link to="/" className="mt-4 inline-block text-pink-400 hover:text-pink-300">
					← Вернуться к списку
				</Link>
			</main>
		)
	}

	return (
		<main className="min-h-screen bg-zinc-950 p-6 text-white">
			<Link to="/" className="text-pink-400 hover:text-pink-300">
				← К списку Валер
			</Link>

			<h1 className="mt-6 text-3xl font-bold text-pink-400">{valera.name}</h1>

			<div className="mt-5 space-y-4">
				<p>ID: {valera.id}</p>

				{/* Здоровье */}
				<div>
					<div className="mb-1 flex justify-between">
						<span>Здоровье</span>
						<span>{valera.health}/100</span>
					</div>

					<div className="h-3 w-full rounded-full bg-zinc-700">
						<div className="h-3 rounded-full bg-green-500" style={{ width: `${valera.health}%` }} />
					</div>
				</div>

				<div>
					<div className="mb-1 flex justify-between">
						<span>Алкоголь</span>
						<span>{valera.alcohol}/100</span>
					</div>

					<div className="h-3 w-full rounded-full bg-zinc-700">
						<div
							className="h-3 rounded-full bg-red-500"
							style={{ width: `${valera.alcohol}%` }}
						></div>
					</div>
				</div>
				<div>
					<div className="mb-1 flex justify-between">
						<span>Усталость</span>
						<span>{valera.fatigue}/100</span>
					</div>

					<div className="h-3 w-full rounded-full bg-zinc-700">
						<div
							className="h-3 rounded-full bg-orange-500"
							style={{ width: `${valera.fatigue}%` }}
						/>
					</div>
				</div>
				<div>
					<div className="mb-1 flex justify-between">
						<span>Баксы</span>
						<span>{valera.money}/100</span>
					</div>

					<div className="h-3 w-full rounded-full bg-zinc-700">
						<div
							className="h-3 rounded-full bg-yellow-500-500"
							style={{ width: `${valera.money}$` }}
						/>
					</div>
				</div>
			</div>
		</main>
	)
}
