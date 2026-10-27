export type Valera = {
	id: number
	name: string
	health: number
	alcohol: number
	joy: number
	fatigue: number
	money: number
}

export type Props = {
	valera: Valera
	onDelete: (id: number) => void
}
