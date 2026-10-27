import { Link } from 'react-router-dom'
import type { Props } from '../types/valera'
export function ValeraCard({ valera, onDelete }: Props) {
	return (
		<div className="bg-zinc-900 p-5 m-3 rounded-2xl border border-zinc-700 hover:border-zinc-500 duration-300">
			<Link to={`/valera/${valera.id}`} className="text-2xl font-bold text-pink-400">
				Имя: {valera.name}
			</Link>

			<p>
				ID: <span className="text-green-500">{valera.id}</span>
			</p>
			<p>
				health: <span className="text-green-500">{valera.health}</span>
			</p>
			<p>
				alcohol: <span className="text-green-500">{valera.alcohol}</span>
			</p>
			<p>
				joy: <span className="text-green-500">{valera.joy}</span>
			</p>
			<p>
				fatigue: <span className="text-green-500">{valera.fatigue}</span>
			</p>
			<p>
				money: <span className="text-green-500">{valera.money}</span>
			</p>
			<button
				onClick={() => onDelete(valera.id)}
				className="mt-2 border-2 border-red-900 p-1 bg-red-800 rounded-2xl hover:bg-red-400 duration-200"
			>
				Удалить
			</button>
		</div>
	)
}
