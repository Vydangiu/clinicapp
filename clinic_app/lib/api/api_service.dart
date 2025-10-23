import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ApiService {
  static const String baseUrl = 'http://localhost:5238/api/auth';
  final _storage = const FlutterSecureStorage();

  Future<Map<String, dynamic>?> login(String username, String password) async {
    final url = Uri.parse('$baseUrl/login');
    final res = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'username': username, 'password': password}),
    );

    if (res.statusCode == 200) {
      final data = jsonDecode(res.body);
      await _storage.write(key: 'token', value: data['access_token']);
      return data;
    } else {
      throw Exception('Đăng nhập thất bại (${res.statusCode})');
    }
  }

  Future<Map<String, dynamic>?> register(
    String username,
    String password,
    int roleId,
  ) async {
    final token = await _storage.read(key: 'token');
    final url = Uri.parse('$baseUrl/register');
    final res = await http.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode({
        'username': username,
        'password': password,
        'roleId': roleId,
      }),
    );

    if (res.statusCode == 200) {
      return jsonDecode(res.body);
    } else {
      throw Exception('Tạo tài khoản thất bại (${res.statusCode})');
    }
  }
}
