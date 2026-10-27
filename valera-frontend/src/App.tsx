import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { ValeraList } from './pages/ValeraList'
import { ValeraStats } from './pages/ValeraStats'

function App() {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<ValeraList />} />
				<Route path="/valera/:id" element={<ValeraStats />} />
			</Routes>
		</BrowserRouter>
	)
}

export default App
