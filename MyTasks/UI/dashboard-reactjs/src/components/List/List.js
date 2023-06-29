import React from 'react';
import { DataGrid } from '@mui/x-data-grid';
import Box from '@mui/material/Box';
import { styled } from '@mui/material/styles';
import DeleteIcon from '@mui/icons-material/Delete';
import UpdateIcon from '@mui/icons-material/Update';
import AddIcon from '@mui/icons-material/Add';
import useApiClient from '../../store/useApi';
import paths from '../../store/paths';
import { useState } from 'react';
import Snackbar from '@mui/material/Snackbar';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogActions from '@material-ui/core/DialogActions';
import { Button } from '@mui/material';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import IconButton from '@mui/material/IconButton';
import './List.css';

const mapData = (data) => {
	const result = [];

	if (!data || data.length === 0) return result;

	data.map((elem) => {
		result.push({
			id: elem.id,
			title: elem.title,
			content: elem.content,
			startDate: elem.startDateTime,
			endDate: elem.endDateTime,
			duration: elem.duration,
			ownerName: elem.ownerName
		});
	});

	return result;
};

const StyledGridOverlay = styled('div')(({ theme }) => ({
	display: 'flex',
	flexDirection: 'column',
	alignItems: 'center',
	justifyContent: 'center',
	height: '100%',
	'& .ant-empty-img-1': {
		fill: theme.palette.mode === 'light' ? '#aeb8c2' : '#262626'
	},
	'& .ant-empty-img-2': {
		fill: theme.palette.mode === 'light' ? '#f5f5f7' : '#595959'
	},
	'& .ant-empty-img-3': {
		fill: theme.palette.mode === 'light' ? '#dce0e6' : '#434343'
	},
	'& .ant-empty-img-4': {
		fill: theme.palette.mode === 'light' ? '#fff' : '#1c1c1c'
	},
	'& .ant-empty-img-5': {
		fillOpacity: theme.palette.mode === 'light' ? '0.8' : '0.08',
		fill: theme.palette.mode === 'light' ? '#f5f5f5' : '#fff'
	}
}));

function CustomNoRowsOverlay() {
	return (
		<StyledGridOverlay>
			<svg width="120" height="100" viewBox="0 0 184 152" aria-hidden focusable="false">
				<g fill="none" fillRule="evenodd">
					<g transform="translate(24 31.67)">
						<ellipse className="ant-empty-img-5" cx="67.797" cy="106.89" rx="67.797" ry="12.668" />
						<path
							className="ant-empty-img-1"
							d="M122.034 69.674L98.109 40.229c-1.148-1.386-2.826-2.225-4.593-2.225h-51.44c-1.766 0-3.444.839-4.592 2.225L13.56 69.674v15.383h108.475V69.674z"
						/>
						<path
							className="ant-empty-img-2"
							d="M33.83 0h67.933a4 4 0 0 1 4 4v93.344a4 4 0 0 1-4 4H33.83a4 4 0 0 1-4-4V4a4 4 0 0 1 4-4z"
						/>
						<path
							className="ant-empty-img-3"
							d="M42.678 9.953h50.237a2 2 0 0 1 2 2V36.91a2 2 0 0 1-2 2H42.678a2 2 0 0 1-2-2V11.953a2 2 0 0 1 2-2zM42.94 49.767h49.713a2.262 2.262 0 1 1 0 4.524H42.94a2.262 2.262 0 0 1 0-4.524zM42.94 61.53h49.713a2.262 2.262 0 1 1 0 4.525H42.94a2.262 2.262 0 0 1 0-4.525zM121.813 105.032c-.775 3.071-3.497 5.36-6.735 5.36H20.515c-3.238 0-5.96-2.29-6.734-5.36a7.309 7.309 0 0 1-.222-1.79V69.675h26.318c2.907 0 5.25 2.448 5.25 5.42v.04c0 2.971 2.37 5.37 5.277 5.37h34.785c2.907 0 5.277-2.421 5.277-5.393V75.1c0-2.972 2.343-5.426 5.25-5.426h26.318v33.569c0 .617-.077 1.216-.221 1.789z"
						/>
					</g>
					<path
						className="ant-empty-img-3"
						d="M149.121 33.292l-6.83 2.65a1 1 0 0 1-1.317-1.23l1.937-6.207c-2.589-2.944-4.109-6.534-4.109-10.408C138.802 8.102 148.92 0 161.402 0 173.881 0 184 8.102 184 18.097c0 9.995-10.118 18.097-22.599 18.097-4.528 0-8.744-1.066-12.28-2.902z"
					/>
					<g className="ant-empty-img-4" transform="translate(149.65 15.383)">
						<ellipse cx="20.654" cy="3.167" rx="2.849" ry="2.815" />
						<path d="M5.698 5.63H0L2.898.704zM9.259.704h4.985V5.63H9.259z" />
					</g>
				</g>
			</svg>
			<Box sx={{ mt: 1 }}>No Rows</Box>
		</StyledGridOverlay>
	);
}

export default function List({ tasksProp }) {
	const [ snackbarOpen, setSnackbarOpen ] = useState(false);
	const [ snackbarMessage, setSnackbarMessage ] = useState('');
	const { response, loading, sendRequest } = useApiClient();
	const [ tasks, setTasks ] = useState(tasksProp);
	const [ dialogOpen, setDialogOpen ] = useState(false);
	const [ dialogMode, setDialogMode ] = useState('add');
	const [ dialogTask, setDialogTask ] = useState(null);
	const [ editedTask, setEditedTask ] = useState({
		id: '',
		title: '',
		content: '',
		startDate: '',
		endDate: '',
		duration: ''
	});

	const columns = [
		{ field: 'id', headerName: 'ID', width: 70, flex: 0.5 },
		{ field: 'title', headerName: 'Tytuł', width: 70, flex: 1 },
		{ field: 'content', headerName: 'Treść', width: 130, flex: 1 },
		{ field: 'startDate', headerName: 'Data startu', width: 130, flex: 1 },
		{ field: 'endDate', headerName: 'Data końca', width: 130, flex: 1 },
		{ field: 'duration', headerName: 'Trwanie', width: 130, flex: 1 },
		{ field: 'ownerName', headerName: 'Właściciel zadania', width: 130, flex: 1 },
		{
			field: 'actions',
			headerName: 'Akcje',
			width: 130,
			flex: 1,
			renderCell: (params) => (
				<div>
					<IconButton
						variant="outlined"
						color="yellow"
						fontSize="large"
						onClick={() => handleUpdate(params.id)}
					>
						<UpdateIcon />
					</IconButton>
					<IconButton variant="outlined" color="red" fontSize="large" onClick={() => handleDelete(params.id)}>
						<DeleteIcon />
					</IconButton>
				</div>
			)
		}
	];

	const handleDelete = async (taskId) => {
		const requestData = { id: taskId };

		try {
			const { response, loading } = await sendRequest(
				paths.deleteTask.path,
				paths.deleteTask.method,
				requestData
			);

			setSnackbarOpen(true);

			setSnackbarMessage('Zadanie zostało usunięte.');

			var taskDtos = tasks.taskDtos.filter((task) => task.id !== taskId);
			tasks.taskDtos = taskDtos;
			setTasks(tasks);
		} catch (error) {
			setSnackbarOpen(true);
			setSnackbarMessage('Wystąpił błąd podczas usuwania zadania.');
		}
	};

	const handleUpdate = (taskId) => {
		const updatedTask = tasks.taskDtos.find((task) => task.id === taskId);
		setEditedTask(updatedTask);
		setDialogMode('update');
		// Otwórz dialog
		setDialogOpen(true);
		setDialogTask(updatedTask);
	};

	const handleCloseDialog = () => {
		setDialogOpen(false);
		setDialogTask(null);
	};

	const handleUpdateTask = async () => {
		const updatedTask = { ...editedTask, id: dialogTask.id };
		const taskIndex = tasks.taskDtos.findIndex((task) => task.id === editedTask.id);
		try {
			const { response, loading } = await sendRequest(
				paths.updateTask.path,
				paths.updateTask.method,
				updatedTask
			);

			var update = updatedTask;

			setSnackbarOpen(true);

			if (loading) {
				setSnackbarMessage('Aktualizacja...');
			} else {
				setSnackbarMessage('Zadanie zostało zaktualizowane.');
				setDialogOpen(false);

				if (taskIndex !== -1) {
					tasks.taskDtos[taskIndex] = update;

					setTasks(tasks);

					handleCloseDialog();
				}
			}
		} catch (error) {
			console.log(error.message);
			setSnackbarMessage('Wystąpił błąd podczas aktualizowania zadania.');
		}
	};

	const handleAdd = () => {
		setDialogMode('add');
		setDialogOpen(true);
		setDialogTask(editedTask);
	};

	const handleAddTask = async () => {
		try {
			const { response, loading } = await sendRequest(paths.addTask.path, paths.addTask.method, editedTask);

			setSnackbarOpen(true);

			if (loading) {
				setSnackbarMessage('Dodawanie...');
			} else {
				setSnackbarMessage('Zadanie zostało dodane.');

				tasks.taskDtos.unshift(editedTask);
				setTasks(tasks);

				handleCloseDialog();
			}
		} catch (error) {
			console.log(error.message);
			setSnackbarMessage('Wystąpił błąd podczas dodawania zadania.');
		}
	};

	const handleCloseSnackbar = () => {
		setSnackbarOpen(false);
	};

	const useStyles = makeStyles((theme) => ({
		dialogPaper: {
			padding: theme.spacing(2),
			minWidth: '300px',
			maxWidth: '600px'
		},
		dialogTitle: {
			paddingBottom: theme.spacing(2),
			fontWeight: 'bold',
			color: '#2a2b36'
		},
		dialogContent: {
			paddingTop: theme.spacing(2)
		},
		dataItem: {
			marginBottom: theme.spacing(1)
		},
		paperRoot: {
			backgroundColor: 'white !important',
			padding: theme.spacing(2),
			marginBottom: theme.spacing(2)
		}
	}));

	const handleChange = (property) => (event) => {
		setEditedTask((prevTask) => ({
			...prevTask,
			[property]: event.target.value
		}));
	};

	const classes = useStyles();

	return (
		<div>
			<div style={{ height: 400, width: '100%' }}>
				<IconButton color="primary" aria-label="Dodaj" onClick={handleAdd}>
					<AddIcon />
				</IconButton>
				<DataGrid
					slots={{
						noRowsOverlay: CustomNoRowsOverlay
					}}
					rows={mapData(tasks.taskDtos)}
					columns={columns}
					width="100%"
					initialState={{
						pagination: {
							paginationModel: { page: 0, pageSize: 5 }
						}
					}}
					pageSizeOptions={[ 5, 10 ]}
				/>
			</div>
			<Snackbar
				open={snackbarOpen}
				autoHideDuration={3000}
				onClose={handleCloseSnackbar}
				message={snackbarMessage}
			/>
			<Dialog open={dialogOpen} onClose={handleCloseDialog} classes={{ paper: classes.dialogPaper }}>
				<DialogTitle color="#2a2b36" className={classes.dialogTitle}>
					Edytuj zadanie
				</DialogTitle>
				<DialogContent className={classes.dialogContent}>
					{dialogTask && (
						<form>
							<TextField
								label="Tytuł"
								fullWidth
								value={editedTask.title}
								onChange={handleChange('title')}
								margin="normal"
								InputLabelProps={{
									style: { color: '#2a2b36', fontSize: '16px' }
								}}
								InputProps={{
									style: { color: '#2a2b36', fontSize: '14px' }
								}}
							/>
							<TextField
								label="Treść"
								fullWidth
								value={editedTask.content}
								onChange={handleChange('content')}
								margin="normal"
								InputLabelProps={{
									style: { color: '#2a2b36', fontSize: '16px' }
								}}
								InputProps={{
									style: { color: '#2a2b36', fontSize: '14px' }
								}}
							/>
							<TextField
								label="Data startu"
								fullWidth
								value={editedTask.startDateTime}
								onChange={handleChange('startDate')}
								margin="normal"
								InputLabelProps={{
									style: { color: '#2a2b36', fontSize: '16px' }
								}}
								InputProps={{
									style: { color: '#2a2b36', fontSize: '14px' }
								}}
							/>
							<TextField
								label="Data końca"
								fullWidth
								value={editedTask.endDateTime}
								onChange={handleChange('endDate')}
								margin="normal"
								InputLabelProps={{
									style: { color: '#2a2b36', fontSize: '16px' }
								}}
								InputProps={{
									style: { color: '#2a2b36', fontSize: '14px' }
								}}
							/>
							<TextField
								label="Trwanie"
								fullWidth
								value={editedTask.duration}
								onChange={handleChange('duration')}
								margin="normal"
								InputLabelProps={{
									style: { color: '#2a2b36', fontSize: '16px' }
								}}
								InputProps={{
									style: { color: '#2a2b36', fontSize: '14px' }
								}}
							/>
						</form>
					)}
				</DialogContent>
				<DialogActions>
					<Box mr={2}>
						<Button onClick={handleCloseDialog} color="primary" variant="outlined">
							Zamknij
						</Button>
					</Box>
					<Box>
						{dialogMode === 'update' ? (
							<Button onClick={handleUpdateTask} color="primary" variant="outlined">
								Aktualizuj
							</Button>
						) : (
							<Button onClick={handleAddTask} color="primary" variant="outlined">
								Dodaj
							</Button>
						)}
					</Box>
				</DialogActions>
			</Dialog>
		</div>
	);
}
