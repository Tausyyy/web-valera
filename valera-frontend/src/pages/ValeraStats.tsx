import { useParams } from 'react-router-dom'

export function ValeraStats() {
	const { id } = useParams() // ну короч юз парамс нужно чтобы читать параметры из адреса страницы, ну типо если адрес такой http://localhost:5173/valera/5, то id будет СТРОКОЙ 5

	return (
		<div>
			<h1>Статистика Валеры</h1>
			<p>ID персонажа: {id}</p>
		</div>
	)
}
