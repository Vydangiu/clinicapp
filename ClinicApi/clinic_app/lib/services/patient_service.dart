import 'dart:convert';
import 'package:http/http.dart' as http;
import '../config/api_config.dart';
import 'token_storage.dart';

class PatientService {
  final _store = TokenStorage();

  Future<Map<String, String>> _header() async {
    final t = await _store.read();
    return {
      'Content-Type': 'application/json',
      if (t != null) 'Authorization': 'Bearer $t',
    };
  }

  Future<List<Map<String, dynamic>>> getAll() async {
    final res = await http.get(
      Uri.parse(ApiConfig.patients),
      headers: await _header(),
    );
    if (res.statusCode == 200) {
      final data = jsonDecode(res.body) as List;
      return data.cast<Map<String, dynamic>>();
    }
    throw Exception('Không tải được bệnh nhân (${res.statusCode})');
  }

  Future<Map<String, dynamic>> create(Map<String, dynamic> p) async {
    final res = await http.post(
      Uri.parse(ApiConfig.patients),
      headers: await _header(),
      body: jsonEncode(p),
    );
    if (res.statusCode == 200 || res.statusCode == 201) {
      return jsonDecode(res.body);
    }
    throw Exception('Tạo bệnh nhân thất bại (${res.statusCode})');
  }

  Future<Map<String, dynamic>> update(int id, Map<String, dynamic> p) async {
    final res = await http.put(
      Uri.parse('${ApiConfig.patients}/$id'),
      headers: await _header(),
      body: jsonEncode(p),
    );
    if (res.statusCode == 200) return jsonDecode(res.body);
    throw Exception('Cập nhật thất bại (${res.statusCode})');
  }

  Future<void> delete(int id) async {
    final res = await http.delete(
      Uri.parse('${ApiConfig.patients}/$id'),
      headers: await _header(),
    );
    if (res.statusCode != 200) {
      throw Exception('Xoá thất bại (${res.statusCode})');
    }
  }

  Future<void> logout() async {
    // ignore: no_leading_underscores_for_local_identifiers
    final _store = TokenStorage();
    await _store.clear();
  }
}
