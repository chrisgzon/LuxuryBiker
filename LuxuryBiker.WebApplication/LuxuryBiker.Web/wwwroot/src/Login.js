import React, { useState } from 'react';
import $ from 'jquery';
import Main from './components/Main';
import { Link } from 'react-router-dom';

const Login = ({ loggin, mostrarAlert }) => {
	const [login, setLogin] = useState({
		username: 'Email...',
		password: 'Password...',
	});

	const handleInputChange = (event) => {
		setLogin({
			...login,
			[event.target.name]: event.target.value,
		});
	};

	const handleSubmit = async (e) => {
		e.preventDefault();

		try {
			await loggin(login.username, login.password);
		} catch (error) {
			// mostrarAlert(error.response.data)
			console.log(error);
		}
	};

	$("body").addClass("bg-gradient-primary");

	return (
		<div className="container">
		{/* Outer Row */}
			<div className="row justify-content-center">
				<div className="col-xl-10 col-lg-12 col-md-9">
					<div className="card o-hidden border-0 shadow-lg my-5">
						<div className="card-body p-0">
							{/* className Nested Row within Card Body */}
							<div className="row">
								<div className="col-lg-6 d-none d-lg-block bg-login-image"></div>
								<div className="col-lg-6">
									<div className="p-5">
										<div className="text-center">
											<h1 className="h4 text-gray-900 mb-4">¡Bienvenido de vuelta!</h1>
										</div>
										<form className="user" onSubmit={handleSubmit}>
											<div className="form-group">
												<input className="form-control form-control-user"
														type='email'
														name='username'
														placeholder={login.username}
														onChange={handleInputChange}
														aria-describedby="emailHelp"
														required
													/>
											</div>
											<div className="form-group">
												<input className="form-control form-control-user"
													id="exampleInputPassword"
													type='password'
													name='password'
													placeholder={login.password}
													required
													onChange={handleInputChange} />
											</div>
											<div className="form-group">
												<div className="custom-control custom-checkbox small">
													<input type="checkbox" className="custom-control-input" id="customCheck" />
													<label className="custom-control-label" htmlFor="customCheck">Recordarme</label>
												</div>
											</div>
											<button className="btn btn-primary btn-user btn-block" type="submit">
												Ingresar
											</button>
										</form>
										<hr />
										<div className="text-center">
											<p className="small"><Link to="/Index">¿Olvidaste tu contraseña?</Link></p>
										</div>
										<div className="text-center">
											<p className="small"><Link to="/Index">¡Crear una cuenta!</Link></p>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default Login;