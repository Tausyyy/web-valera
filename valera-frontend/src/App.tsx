import { useEffect, useState } from 'react'
import { createValera, deleteValeras, getValeras } from './api/valeraApi'
import { ValeraCard } from './components/valera_card'
import type { Valera } from './types/valera'

function App() {
	const [newName, setNewName] = useState('')
	const [valeraList, setValeraList] = useState<Valera[]>([])
	const [search, setSearch] = useState('')
	const [isLoading, setIsLoading] = useState(true)
	const [error, setError] = useState('')

	useEffect(() => {
		async function loadValeras() {
			setIsLoading(true)
			setError('')

			try {
				const data = await getValeras()
				setValeraList(data)
			} catch (error) {
				console.error('Ошибка обращения к API', error)
				setError('Не удалось загрузить Валер. Проверь подключение к серверу.')
			} finally {
				setIsLoading(false)
			}
		}

		loadValeras()
	}, [])

	async function addValera() {
		if (!newName.trim()) {
			return
		}

		try {
			const createdValera = await createValera(newName.trim())
			setValeraList(prev => [...prev, createdValera])
			setNewName('')
			setError('')
		} catch (error) {
			console.error('Не удалось создать Валеру', error)
			setError('Не удалось создать Валеру. Попробуй ещё раз.')
		}
	}

	async function handleDelete(id: number) {
		try {
			await deleteValeras(id)
			setValeraList(prev => prev.filter(valera => valera.id !== id))
		} catch (error) {
			console.error('Не удалось удалить Валеру', error)
			setError('Не удалось удалить Валеру. Попробуй ещё раз.')
		}
	}

	const filteredValeras = valeraList.filter(valera =>
		valera.name.toLowerCase().includes(search.toLowerCase())
	)

	return (
		<main className="min-h-screen bg-zinc-950 text-white">
			<div className="mx-auto max-w-6xl px-4 py-10 sm:px-6 lg:px-8">
				<header className="mb-10">
					<h1 className="text-4xl font-black tracking-tight sm:text-5xl">
						Пьяный Валера<span className="text-pink-500">.</span>
					</h1>

					<p className="mt-3 max-w-xl text-zinc-400">
						Это что-то больше чем просто водка пять озер.
					</p>
				</header>

				<section className="mb-8 rounded-2xl border border-zinc-800 bg-zinc-900/70 p-5 sm:p-6">
					<h2 className="mb-4 text-lg font-bold">Создать персонажа</h2>

					<form
						className="flex flex-col gap-3 sm:flex-row"
						onSubmit={event => {
							event.preventDefault()
							addValera()
						}}
					>
						<input
							type="text"
							placeholder="Введите имя Валеры..."
							value={newName}
							onChange={event => setNewName(event.target.value)}
							className="min-w-0 flex-1 rounded-xl border border-zinc-700 bg-zinc-950 px-4 py-3 outline-none transition placeholder:text-zinc-500 focus:border-pink-500 focus:ring-2 focus:ring-pink-500/20"
						/>

						<button
							type="submit"
							disabled={!newName.trim()}
							className="rounded-xl bg-pink-500 px-5 py-3 font-semibold text-white transition hover:bg-pink-400 disabled:cursor-not-allowed disabled:opacity-40"
						>
							Добавить Валеру
						</button>
					</form>
				</section>

				<section>
					<div className="mb-5 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
						<h2 className="text-2xl font-bold">Список персонажей</h2>

						<span className="w-fit rounded-full border border-zinc-800 bg-zinc-900 px-3 py-1 text-sm text-zinc-400">
							Всего: {valeraList.length}
						</span>
					</div>

					<input
						type="text"
						placeholder="Поиск по имени..."
						value={search}
						onChange={event => setSearch(event.target.value)}
						className="mb-6 w-full rounded-xl border border-zinc-800 bg-zinc-900 px-4 py-3 outline-none transition placeholder:text-zinc-500 focus:border-pink-500 focus:ring-2 focus:ring-pink-500/20"
					/>

					{error && (
						<div
							role="alert"
							className="mb-6 rounded-xl border border-red-900/60 bg-red-950/30 p-4 text-sm text-red-300"
						>
							{error}
						</div>
					)}

					{isLoading ? (
						<div className="flex flex-col items-center py-20 text-zinc-400">
							<div className="h-10 w-10 animate-spin rounded-full border-4 border-zinc-700 border-t-pink-500" />
							<p className="mt-4">Загружаем Валер...</p>
						</div>
					) : error && valeraList.length === 0 ? (
						<div className="rounded-2xl border border-zinc-800 bg-zinc-900/50 p-10 text-center">
							<h3 className="text-xl font-bold">Не удалось загрузить список</h3>
							<p className="mt-2 text-zinc-400">Проверь, запущен ли сервер API.</p>
						</div>
					) : valeraList.length === 0 ? (
						<div className="rounded-2xl border border-dashed border-zinc-700 bg-zinc-900/30 p-12 text-center">
							<div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-2xl bg-pink-500/10 text-2xl text-pink-400">
								+
							</div>
							<h3 className="text-xl font-bold">Пока здесь пусто</h3>
							<p className="mt-2 text-zinc-400">Создай своего первого Валеру!</p>
						</div>
					) : filteredValeras.length === 0 ? (
						<div className="rounded-2xl border border-zinc-800 bg-zinc-900/50 p-10 text-center">
							<h3 className="text-xl font-bold">Ничего не найдено</h3>
							<p className="mt-2 text-zinc-400">Попробуй изменить поисковый запрос.</p>
						</div>
					) : (
						<div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3">
							{filteredValeras.map(valera => (
								<ValeraCard key={valera.id} valera={valera} onDelete={handleDelete} />
							))}
						</div>
					)}
				</section>

				<footer className="mt-12 border-t border-zinc-800 pt-5 text-sm text-zinc-500">
					Valera · Персонажей: {valeraList.length}
				</footer>
			</div>
		</main>
	)
}

export default App
